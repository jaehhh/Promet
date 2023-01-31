using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealKit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
     if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<MoveController>().PlayerHeal();
            GetComponent<SpriteRenderer>().enabled = false;

            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<MoveController>().PlayerHeal();
            GetComponent<SpriteRenderer>().enabled = false;

            GetComponent<AudioSource>().Play();
        }
    }
}
