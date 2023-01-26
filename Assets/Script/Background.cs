using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    // 참고 객체
    private Camera cam;
    private CameraMove cameraMove;
    private MoveController moveController;
    private float runSpeed;

    // 설정값
    private float scrollSpeed = 0.25f;

    // 메모리
    public float addX;

    private void Awake()
    {
        cam = Camera.main;
        cameraMove = cam.GetComponent<CameraMove>();
        moveController = GameObject.Find("Person").GetComponent<MoveController>();
        runSpeed = moveController.RunSpeed;

        StartCoroutine("BackgroundMove");
    }

    private IEnumerator BackgroundMove()
    {
        while(true)
        {
            if(cameraMove.isSeparate && !cameraMove.cameraStop) // 대쉬로 인해 카메라가 분리되면 배경도 분리
            {
                addX -= scrollSpeed * runSpeed * Time.deltaTime;
            }
            else if(cameraMove.isSeparate && cameraMove.cameraStop) // 카메라 이동안할 때 배경도 이동X
            {
            }
            else
            {
                addX -= scrollSpeed * moveController.speed * moveController.AddSpeed * Time.deltaTime;
            }

            this.transform.position = new Vector2(cam.transform.position.x + addX, 0);

            if (transform.position.x <= cam.transform.position.x - 19.2f)
            {
                transform.position = new Vector2(cam.transform.position.x + 19.2f, 0);
                addX = 19.2f;
            }

            yield return null;
        }
    }

    public void StopBackgroundMove()
    {
        StopAllCoroutines();
    }
}
