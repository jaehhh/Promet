using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private Animator anim;
    public bool canUseItem;
    [SerializeField]
    private GameObject[] itemPrefabs; // ������ ����
    [SerializeField]
    private float[] itemPercent; // ���빰 Ȯ��
    private GameObject innerItem; // �����۹ڽ� �ȿ� �ִ� ���빰


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (canUseItem) return;

        if (collision.tag == "PlayerProjectile" || collision.tag == "PlayerAttack")
        {
            // anim.SetTrigger("unbox");

            canUseItem = true;

            // Ȯ�� �����ϸ鼭 �����۴� 0~100 ���� Ư�� ���� ��ġ ���� ����
            for (int i = 1; i < itemPercent.Length; ++i)
            {
                itemPercent[i] += itemPercent[i - 1];
            }

            float value;

            while (true)
            {
                value = Random.Range(0f, 100f);

                if (value != 0) break;
            }

            for (int i = 1; i < itemPercent.Length; ++i)
            {
                if (itemPercent[i - 1] < value && value <= itemPercent[i])
                {
                    GameObject clone = Instantiate(itemPrefabs[i - 1], transform.position, Quaternion.identity);
                    innerItem = clone;

                    break;
                }
            }

            this.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
