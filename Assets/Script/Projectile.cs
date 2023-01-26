using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public float speed = 20;
    [SerializeField]
    private bool isMine = true;
    [SerializeField]
    private GameObject boomEffect;

    private void Update()
    {
        transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMine)
        {
            if (collision.tag == "MonsterProjectile")
            {
                DoHit();
            }
            else if(collision.tag == "Monster")
            {
                DoHit();
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                bool isHit = collision.transform.GetComponent<MoveController>().PlayerHit();

                if (isHit) DoHit();
            }
            else if (collision.tag == "PlayerProjectile")
            {
                DoHit();
            }
            else if(collision.tag == "PlayerAttack")
            {
                Destroy(gameObject);
            }
        }
    }

    public void DoHit()
    {
        Instantiate(boomEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void DoDisappear()
    {
        Destroy(gameObject);
    }
}
