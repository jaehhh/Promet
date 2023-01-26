using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private enum playerCollisionType {none, heal, hit, block} // ĳ���Ϳ� ���˽� 
    private enum attackCollisionType {none, throgh, block, broken} // ���ݴ��ҽ�

    [SerializeField]
    private playerCollisionType playerCollisionType_;
    [SerializeField]
    private attackCollisionType attackCollisionType_;

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
                    Destroy(gameObject);
                    break;

                case (playerCollisionType.hit):
                    bool isHit = collision.transform.GetComponent<MoveController>().PlayerHit();
                    if (isHit) Destroy(gameObject);
                    break;

                case (playerCollisionType.block):
                    // �÷��̾� �ӵ� 0
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
                    // ���̾�� �浹���� ����
                    break;

                case (attackCollisionType.block):
                    if (collision.tag == "PlayerAttack") break;
                    Destroy(collision.gameObject);
                    break;

                case (attackCollisionType.broken):
                    Destroy(gameObject);
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
                    Destroy(gameObject);
                    break;

                case (playerCollisionType.hit):
                    bool isHit = collision.GetComponent<MoveController>().PlayerHit();
                    if (isHit) Destroy(gameObject);
                    break;

                case (playerCollisionType.block):
                    // �÷��̾� �ӵ� 0
                    break;

                default:
                    break;
            }
        }
    }
}