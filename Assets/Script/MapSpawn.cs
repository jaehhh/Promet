using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject map;
    private Transform cam;

    private float removeAtPosX;
    private float spawnPosX;

    // �ʽ���
    private Transform[] maps;
    private int mapIndex;

    private void Awake()
    {
        maps = new Transform[4];

        cam = Camera.main.transform;

    }

    private void Update()
    {
        PlayerCheck();
    }

    private void SpawnNewMap()
    {
        spawnPosX += 19.2f;

        // ���� ����
        int nextIndex = mapIndex + 1;
        if (nextIndex >= maps.Length) nextIndex = 0;

        if(maps[nextIndex] != null) // �Ѿ ����� �ٽ� ������
        {
            maps[mapIndex] = maps[nextIndex];
            maps[nextIndex].transform.position = new Vector2(spawnPosX, 0);
        }
        else // ��� ����. �ִ� ���� ũ�⸸ŭ
        {
            GameObject temp = GameObject.Instantiate(map);
            temp.transform.position = new Vector2(spawnPosX, 0);

            maps[mapIndex] = temp.transform;
        }
        mapIndex++;
        if (mapIndex >= maps.Length) mapIndex = 0;
        maps[mapIndex] = null;

        removeAtPosX += 19.2f; // ī�޶��� �� ũ�⿡ ���� ��ġ �ٸ�
    }

    private void PlayerCheck()
    {
        if(cam.position.x >= removeAtPosX)
        {
            SpawnNewMap();
        }
    }
}
