using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private AttackController attackController;
    private MoveController moveContoller;
    private IEnumerator cor;
    private Rigidbody2D rigid; // 플레이어 rigid
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
            Debug.LogWarning($"그라운드 검출되었지만 공중상태");
            debuging = true;
        }
        
    }

    // 점프 상승중일 때는 충돌X, 하강일 때 땅과 충돌
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

            if (moveContoller.isJump)//moveContoller.grounds++ == 0) // 방금 땅과 첫 접촉이면
            {
                moveContoller.SetIsJump(false);
                moveContoller.isJump = false;

                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.2f); // 땅속 뚫는 버그 방지 속도 감속
            }

            //if (moveContoller.grounds >= 1 && )
        }

   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (moveContoller.grounds >= 3 || debuging)
        {
            Debug.LogWarning($"{collision.name}의 부모 : {collision.transform.parent.position}, 위치 {collision.transform.position}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.tag != "PlayerFootTrigger") return;

        if (collision.transform.tag == "Ground")
        {
            moveContoller.grounds -= 1;
            if (moveContoller.grounds < 0) Debug.LogWarning($"ground 음수");

            if (moveContoller.grounds == 0)
            {
                moveContoller.SetIsJump(true);
                moveContoller.isJump = true;

                Jump();
            }
        }
    }
    // 아래에서 위쪽 플랫폼으로 점프하기 위한 코드
    public void Jump()
    {
        if(cor != null)
        StopCoroutine(cor);
        cor = JumpToNextGround();
        StartCoroutine(cor);
    }
}
