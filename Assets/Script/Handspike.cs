using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handspike : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;

    private bool activation;
    private float currentZ; // 처음 값
    private float maxZ; // 도달해야하는 값
    private float rotateSpeed = 5f;

    private void Awake()
    {
        currentZ = transform.eulerAngles.z;

        Debug.Log(currentZ);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activation) return;

        if (collision.tag == "Player" || collision.tag == "PlayerFoot")
        {
            RotateHandle();
        }

    }

    private void RotateHandle()
    {
        if(!activation)
        {
            activation = true;

            maxZ = currentZ * -1;
        }
        

        currentZ -= rotateSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0,0, currentZ);


        if(currentZ > maxZ)
        {
            RotateHandle();
        }
        else
        {
            currentZ = maxZ;

            Instantiate(itemPrefab, transform.position + Vector3.up * 9f + Vector3.right * 17f, Quaternion.identity);
        }
    }
}
