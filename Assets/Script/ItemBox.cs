using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private Animator anim;
    public bool canUseItem;
    [SerializeField]
    private GameObject[] itemPrefabs; // 아이템 종류
    [SerializeField]
    private float[] itemPercent; // 내용물 확률
    private GameObject innerItem; // 아이템박스 안에 있는 내용물


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

            // 확률 누적하면서 아이템당 0~100 사이 특정 범위 수치 값을 정함
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
