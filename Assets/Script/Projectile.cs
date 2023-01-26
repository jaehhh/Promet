using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public float speed = 20;
    [SerializeField]
    private bool isMine = true;

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
                Destroy(gameObject);
            }
            else if(collision.tag == "Monster")
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if(collision.tag == "Player")
            {
                bool isHit = collision.transform.GetComponent<MoveController>().PlayerHit();

                if(isHit) Destroy(gameObject);
            }
            else if (collision.tag == "PlayerProjectile" || collision.tag == "PlayerAttack")
            {
                Destroy(gameObject);
            }
        }
    }
}
