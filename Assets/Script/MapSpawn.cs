using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour
{
    [SerializeField]
    private bool isTest;
    [SerializeField]
    private GameObject[] debugingButton;

    [SerializeField]
    private GameObject map;

    private Transform cam;

    private float removeAtPosX;
    private float spawnPosX;

    // 맵스택
    private Transform[] maps;
    private int mapIndex;

    [SerializeField]
    private GameObject[] obstacleMapPrefab;
    [SerializeField]
    private GameObject[] itemMapPrefab;
    [SerializeField]
    private float[] itemMapSpawnPercent; // 인덱스0 : 0
    [SerializeField]
    private GameObject restMapPrefab; // 평지

    private int spawnTurn = 1;
    private int obstacleRange = 3;
    private int restRange;

    [SerializeField]
    private GameObject flyMonster;
    private Transform clone; // 몬스터 살아있는지 체크
    private float enemySpawnMaxTime = 12f;
    private float enemySpawnCurrentTime;

    private void Awake()
    {
        maps = new Transform[4];

        cam = Camera.main.transform;

        if(isTest == false)
        EnemySpawn();
        else
        {
            for(int i = 0; i < debugingButton.Length; ++i )
            debugingButton[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (cam.position.x >= removeAtPosX) // 플레이어 위치 체크
        {
            if (isTest)
                SpawnNewMap();
            else
                PickMap();
        }
    }


    private void PickMap()
    {
        if (spawnTurn % 4 != 0 || spawnTurn < 4) // 4번째 턴이 아니면
        {
            if(obstacleRange < 3)
            {
                spawnPosX += 19.2f;
                removeAtPosX += 19.2f;

                int index = Random.Range(0, obstacleMapPrefab.Length);
                Instantiate(obstacleMapPrefab[index], new Vector2(spawnPosX, 0), Quaternion.identity);

                ++obstacleRange;
            }
            else
            {
                if(restRange < 3)
                {

                    spawnPosX += 19.2f;
                    removeAtPosX += 19.2f;

                    Instantiate(restMapPrefab, new Vector2(spawnPosX, 0), Quaternion.identity);

                    ++restRange;

                    if(restRange >= 3)
                    {
                        obstacleRange = 0;
                        restRange = 0;
                        ++spawnTurn;
                    }
                }
            }
        }
        else // 4번째 턴
        {
            if(restRange ==0) // 아이템맵
            {
                spawnPosX += 19.2f;
                removeAtPosX += 19.2f;

                // 확률 누적하면서 아이템당 0~100 사이 특정 범위 수치 값을 정함
                for (int i = 1; i < itemMapSpawnPercent.Length; ++i)
                {
                    itemMapSpawnPercent[i] += itemMapSpawnPercent[i - 1];
                }

                float value;

                while (true)
                {
                    value = Random.Range(0f, 100f);

                    if (value != 0) break;
                }

                for (int i = 1; i < itemMapSpawnPercent.Length; ++i)
                {
                    if (itemMapSpawnPercent[i - 1] < value && value <= itemMapSpawnPercent[i])
                    {
                        Instantiate(itemMapPrefab[i - 1], new Vector2(spawnPosX, 0), Quaternion.identity);

                        break;
                    }
                }

                ++restRange;
            }
            else // 이어서 평지 한개
            {
                spawnPosX += 19.2f;
                removeAtPosX += 19.2f;

                Instantiate(restMapPrefab, new Vector2(spawnPosX, 0), Quaternion.identity);

                restRange = 0;
                ++spawnTurn;
            }
        }
    }

    // 같은 맵 반복
    private void SpawnNewMap()
    {
        spawnPosX += 19.2f;

        // 원형 큐
        int nextIndex = mapIndex + 1;
        if (nextIndex >= maps.Length) nextIndex = 0;

        if(maps[nextIndex] != null) // 넘어간 배경을 다시 앞으로
        {
            maps[mapIndex] = maps[nextIndex];
            maps[nextIndex].transform.position = new Vector2(spawnPosX, 0);
        }
        else // 배경 생성. 최대 큐 크기만큼
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

    private void EnemySpawn()
    {
        if (clone == null) enemySpawnCurrentTime += 0.5f;

        if(enemySpawnCurrentTime >= enemySpawnMaxTime)
        {
            clone = Instantiate(flyMonster).transform;

            enemySpawnCurrentTime = 0;
        }

        Invoke("EnemySpawn", 0.5f);
    }
}
