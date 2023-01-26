using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarObstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<MoveController>().hitPillar = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<MoveController>().hitPillar = false;
        }
    }
}
