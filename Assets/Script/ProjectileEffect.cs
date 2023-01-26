using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField]
    private bool isMine;

    private void Awake()
    {
        if (isMine)
            GetComponent<Animator>().SetTrigger("player");

        else
            GetComponent<Animator>().SetTrigger("enemy");
    }

    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}
