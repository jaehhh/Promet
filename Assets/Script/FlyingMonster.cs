using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    private IEnumerator moveCor;
    private bool isDie;

    private int shootTime;
    // private float runSpeed;

    private void Awake()
    {
        // runSpeed = GameObject.Find("Person").GetComponent<MoveController>().RunSpeed;

        moveCor = FlyKeepDistance();
        StartCoroutine(moveCor);

        Invoke("Attack", 1.5f);
    }

    private IEnumerator FlyKeepDistance()
    {
        float addPos = 0;

        while (true)
        {
            if(shootTime >= 2)
            {
                addPos += -0.2f * Time.deltaTime;
            }

            Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(0.9f + addPos, 0.5f));

            if (isDie)
            {
                transform.position = new Vector2(pos.x, transform.position.y);
            }
            else
            {
                transform.position = pos;
            }
            
            yield return null;
        }
    }

    private void Attack()
    {
        if (isDie) return;

        Instantiate(projectile, transform.position, Quaternion.identity);

        ++shootTime;

        Invoke("Attack", 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerProjectile")
        {
            GetComponent<Rigidbody2D>().gravityScale = 2f;
            isDie = true;
            GetComponent<Animator>().SetBool("isDie", true);
        }
        else if(collision.tag == "PlayerAttack")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            StopCoroutine(moveCor);
        }
        if(collision.transform.tag == "Player") // 플레이어 피격
        {
            if(isDie)
            {
                collision.transform.GetComponent<MoveController>().PlayerHit(2);
            }
            else
            {
                collision.transform.GetComponent<MoveController>().PlayerHit();
            }

            Destroy(gameObject);
        }
    }
}
