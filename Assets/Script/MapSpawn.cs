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

    // 맵스택
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

        // 원형 스택
        int nextIndex = mapIndex + 1;
        if (nextIndex >= maps.Length) nextIndex = 0;

        if(maps[nextIndex] != null) // 넘어간 배경을 다시 앞으로
        {
            maps[mapIndex] = maps[nextIndex];
            maps[nextIndex].transform.position = new Vector2(spawnPosX, 0);
        }
        else // 배경 생성. 최대 스택 크기만큼
        {
            GameObject temp = GameObject.Instantiate(map);
            temp.transform.position = new Vector2(spawnPosX, 0);

            maps[mapIndex] = temp.transform;
        }
        mapIndex++;
        if (mapIndex >= maps.Length) mapIndex = 0;
        maps[mapIndex] = null;

        removeAtPosX += 19.2f; // 카메라대비 맵 크기에 따라 수치 다름
    }

    private void PlayerCheck()
    {
        if(cam.position.x >= removeAtPosX)
        {
            SpawnNewMap();
        }
    }
}
