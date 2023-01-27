using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private AttackController attackController;
    private MoveController moveContoller;
    private IEnumerator cor;
    private Rigidbody2D rigid; // �÷��̾� rigid
    [SerializeField]
    private Collider2D footCollider;
    private float ground0Time;

    private bool debuging;

    private void Awake()
    {
        attackController = GetComponentInParent<AttackController>();
        moveContoller = GetComponentInParent<MoveController>();
        rigid = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log($"ground : {moveContoller.grounds}");

        if (moveContoller.grounds >= 1 && moveContoller.isJump)
        {
            Debug.LogWarning($"�׶��� ����Ǿ����� ���߻���");
            debuging = true;
        }
        
    }

    // ���� ������� ���� �浹X, �ϰ��� �� ���� �浹
    private IEnumerator JumpToNextGround()
    {
        GetComponent<Collider2D>().enabled = false;
        footCollider.isTrigger = true;

        yield return new WaitForSeconds(Time.deltaTime * 2);

        while (true)
        {
            if (rigid.velocity.y < 0f)
            {
                GetComponent<Collider2D>().enabled = true;
                footCollider.isTrigger = false;
                break;
            }

            yield return null;
        }

        //StopCoroutine("CheckGround0Bug");
        //  ground0Time = 0;
        //StartCoroutine("CheckGround0Bug");
    }

    private IEnumerator CheckGround0Bug()
    {
        while(ground0Time < 0.15f)
        {
            if(rigid.velocity.y == 0 && !attackController.isDash)
            {
                ground0Time += Time.deltaTime;
            }

            yield return null;
        }

        if (moveContoller.isJump)
        {
            moveContoller.SetIsJump(false);
            moveContoller.isJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.tag != "PlayerFootTrigger") return;

        if (collision.transform.tag == "Ground")
        {
            moveContoller.grounds++;

            if (moveContoller.isJump)//moveContoller.grounds++ == 0) // ��� ���� ù �����̸�
            {
                moveContoller.SetIsJump(false);
                moveContoller.isJump = false;

                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.2f); // ���� �մ� ���� ���� �ӵ� ����
            }

            //if (moveContoller.grounds >= 1 && )
        }

   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (moveContoller.grounds >= 3 || debuging)
        {
            Debug.LogWarning($"{collision.name}�� �θ� : {collision.transform.parent.position}, ��ġ {collision.transform.position}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.tag != "PlayerFootTrigger") return;

        if (collision.transform.tag == "Ground")
        {
            moveContoller.grounds -= 1;
            if (moveContoller.grounds < 0) Debug.LogWarning($"ground ����");

            if (moveContoller.grounds == 0)
            {
                moveContoller.SetIsJump(true);
                moveContoller.isJump = true;

                Jump();
            }
        }
    }
    // �Ʒ����� ���� �÷������� �����ϱ� ���� �ڵ�
    public void Jump()
    {
        if(cor != null)
        StopCoroutine(cor);
        cor = JumpToNextGround();
        StartCoroutine(cor);
    }
}
