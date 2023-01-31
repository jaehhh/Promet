using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject destroyEffect;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootClip;
    [SerializeField]
    private AudioClip hitClip;

    private IEnumerator moveCor;
    private bool isDie;

    private int shootTime;
    // private float runSpeed;

    private void Awake()
    {
        // runSpeed = GameObject.Find("Person").GetComponent<MoveController>().RunSpeed;
        audioSource = GetComponent<AudioSource>();

        StartCoroutine("DownFlyAtFirst");
    }

    private IEnumerator DownFlyAtFirst()
    {
        float posY = 1f;

        while (posY >= 0.4f)
        {
            posY -= 0.7f * Time.deltaTime;

            Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(0.9f , posY));
            transform.position = pos;

            yield return null;
        }

        moveCor = FlyKeepDistance();
        StartCoroutine(moveCor);
        Invoke("Attack", 0.5f);
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

            Vector2 pos = Camera.main.ViewportToWorldPoint(new Vector2(0.9f + addPos, 0.4f));

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

        audioSource.clip = shootClip;
        audioSource.Play();

        Invoke("Attack", 3.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerProjectile")
        {

            GetComponent<Rigidbody2D>().gravityScale = 3.5f;
            GetComponent<Collider2D>().isTrigger = false;
            isDie = true;
            GetComponent<Animator>().SetBool("isDie", true);

            audioSource.clip = hitClip;
            audioSource.Play();
        }
        else if(collision.tag == "PlayerAttack")
        {
            Destroy(gameObject);

            Instantiate(destroyEffect, transform.position, Quaternion.identity);
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
                collision.transform.GetComponent<MoveController>().PlayerHit();
            }
            else
            {
                collision.transform.GetComponent<MoveController>().PlayerHit(2);
            }

            Destroy(gameObject);
        }
    }
}
