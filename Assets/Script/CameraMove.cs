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

    // 카메라 팔로우 이동 - 기둥 exit, 대쉬후 자리복귀
    public void FollowingTarget()
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = FollowingTargetCor();
        StartCoroutine(cor);
    }

    // 카메라 독립 이동 - 대쉬
    public void SeparateMove()
    {
        if (cor != null)
            StopCoroutine(cor);
        cor = SeparateMoveCor();
        StartCoroutine(cor);
    }

    // 팔로잉 카메라
    private IEnumerator FollowingTargetCor()
    {
        while(true)
        {
            if (isSeparate) isSeparate = false;

            this.transform.position = new Vector3(target.position.x + distanceX, 0, -10f);

            yield return null;
        }
    }


    // 독립이동 카메라
    private IEnumerator SeparateMoveCor()
    {
        runSpeed = target.GetComponent<MoveController>().RunSpeed;

        while(true)
        {
            if (!isSeparate)
            {
                isSeparate = true;
            }

            if(moveController.hitPillar && attackController.isDash) // 기둥접촉+대시중
            {
                if (!cameraStop) cameraStop = true;
            }
            else if(moveController.hitPillar && attackController.recoveringSpeed) // 기둥접촉+리커버리중
            {
                if (cameraStop) cameraStop = false;

                transform.position += Vector3.right * runSpeed * Time.deltaTime;
            }
            else if(moveController.hitPillar) //기둥만 접촉
            {
                if (!cameraStop) cameraStop = true;
            }
            else // 아무런 방해가 없을 때
            {
                if (cameraStop) cameraStop = false;

                transform.position += Vector3.right * runSpeed * Time.deltaTime;
            }

            yield return null;
        }
    }
    
    // 죽었을 때
    public void CameraStop() 
    {
        StopAllCoroutines();
    }
}
