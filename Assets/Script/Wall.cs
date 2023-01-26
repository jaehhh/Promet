using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{ collision.name} 의 태그 : {collision.tag}");

      if(collision.tag =="Player")
        {
            Debug.Log("벽충돌");

            collision.GetComponent<MoveController>().PlayerDie();
        }
    }
}
