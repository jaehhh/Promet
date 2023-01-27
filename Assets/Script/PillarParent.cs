using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarParent : MonoBehaviour
{
    [SerializeField]
    private Collider2D groundCollider;

    // ≥¢ø¥¿ª ∂ß ±‚µ’ ≈ª√‚«“ ºˆ ¿÷µµ∑œ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "PlayerFoot")
        {
            GetComponent<Collider2D>().enabled = false;
            groundCollider.enabled = false;

            //collision.transform.position = new Vector2(transform.GetChild(0).transform.position.x - 1f, collision.transform.position.y);
        }
    }
}
