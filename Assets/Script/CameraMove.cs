using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform target;
    private MoveController moveController;
    private AttackController attackController;
    private float runSpeed;
    [HideInInspector]
    public float distanceX = 6.6f;
    public bool isSeparate;
    public bool cameraStop;

    private IEnumerator cor;

    private void Awake()
    {
       target = GameObject.Find("Person").transform;
        moveController = target.GetComponent<MoveController>();
        attackController = target.GetComponent<AttackController>();

        FollowingTarget();
    }

    // ī�޶� �ȷο� �̵� - ��� exit, �뽬�� �ڸ�����
    public void FollowingTarget()
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = FollowingTargetCor();
        StartCoroutine(cor);
    }

    // ī�޶� ���� �̵� - �뽬
    public void SeparateMove()
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = SeparateMoveCor();
        StartCoroutine(cor);
    }

    // �ȷ��� ī�޶�
    private IEnumerator FollowingTargetCor()
    {
        while(true)
        {
            if (isSeparate) isSeparate = false;

            this.transform.position = new Vector3(target.position.x + distanceX, 0, -10f);

            yield return null;
        }
    }


    // �����̵� ī�޶�
    private IEnumerator SeparateMoveCor()
    {
        runSpeed = target.GetComponent<MoveController>().RunSpeed;

        while(true)
        {
            if (!isSeparate)
            {
                isSeparate = true;
            }

            if(moveController.hitPillar && attackController.isDash) // �������+�����
            {
                if (!cameraStop) cameraStop = true;
            }
            else if(moveController.hitPillar && attackController.recoveringSpeed) // �������+��Ŀ������
            {
                if (cameraStop) cameraStop = false;

                transform.position += Vector3.right * runSpeed * Time.deltaTime;
            }
            else if(moveController.hitPillar) //��ո� ����
            {
                if (!cameraStop) cameraStop = true;
            }
            else // �ƹ��� ���ذ� ���� ��
            {
                if (cameraStop) cameraStop = false;

                transform.position += Vector3.right * runSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }
    
    // �׾��� ��
    public void CameraStop() 
    {
        StopAllCoroutines();
    }
}
