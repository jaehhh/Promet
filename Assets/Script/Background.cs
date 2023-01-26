using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    // ���� ��ü
    private Camera cam;
    private CameraMove cameraMove;
    private MoveController moveController;
    private float runSpeed;

    // ������
    private float scrollSpeed = 0.25f;

    // �޸�
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
            if(cameraMove.isSeparate && !cameraMove.cameraStop) // �뽬�� ���� ī�޶� �и��Ǹ� ��浵 �и�
            {
                addX -= scrollSpeed * runSpeed * Time.deltaTime;
            }
            else if(cameraMove.isSeparate && cameraMove.cameraStop) // ī�޶� �̵����� �� ��浵 �̵�X
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
