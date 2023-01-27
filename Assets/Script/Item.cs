using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private enum playerCollisionType {none, heal, hit, block} // 캐릭터와 접촉시 
    private enum attackCollisionType {none, throgh, block, broken} // 공격당할시

    [SerializeField]
    private playerCollisionType playerCollisionType_;
    [SerializeField]
    private attackCollisionType attackCollisionType_;

    public void Setup()
    {
        if(!gameObject.activeSelf)
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Collider2D>().isTrigger = true;
        }

        if(collision.transform.tag == "Player")
        {
            switch (playerCollisionType_)
            {
                case (playerCollisionType.heal):
                    collision.transform.GetComponent<MoveController>().PlayerHeal();
                    gameObject.SetActive(false);
                    break;

                case (playerCollisionType.hit):
                    bool isHit = collision.transform.GetComponent<MoveController>().PlayerHit();
                    if (isHit) gameObject.SetActive(false);
                    break;

                case (playerCollisionType.block):
                    // 플레이어 속도 0
                    break;

                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlayerAttack" || collision.transform.tag == "PlayerProjectile")
        {
            switch (attackCollisionType_)
            {
                case (attackCollisionType.throgh):
                    // 레이어에서 충돌판정 관리
                    break;

                case (attackCollisionType.block):
                    if (collision.tag == "PlayerAttack") break;
                    collision.GetComponent<Projectile>().DoHit();
                    break;

                case (attackCollisionType.broken):
                    gameObject.SetActive(false);
                    break;

                default:
                    break;
            }
        }

        if (collision.transform.tag == "Player")
        {
            switch (playerCollisionType_)
            {
                case (playerCollisionType.heal):
                    collision.GetComponent<MoveController>().PlayerHeal();
                    gameObject.SetActive(false);
                    break;

                case (playerCollisionType.hit):
                    bool isHit = collision.GetComponent<MoveController>().PlayerHit();
                    if (isHit) gameObject.SetActive(false);
                    break;

                case (playerCollisionType.block):
                    // 플레이어 속도 0
                    break;

                default:
                    break;
            }
        }
    }
}
