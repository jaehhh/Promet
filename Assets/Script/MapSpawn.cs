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

    // �ʽ���
    private Transform[] maps;
    private int mapIndex;

    [SerializeField]
    private GameObject[] obstacleMapPrefab;
    [SerializeField]
    private GameObject[] itemMapPrefab;
    [SerializeField]
    private float[] itemMapSpawnPercent; // �ε���0 : 0
    [SerializeField]
    private GameObject restMapPrefab; // ����

    private int spawnTurn = 1;
    private int obstacleRange = 3;
    private int restRange;

    [SerializeField]
    private GameObject flyMonster;
    private Transform clone; // ���� ����ִ��� üũ
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
        if (cam.position.x >= removeAtPosX) // �÷��̾� ��ġ üũ
        {
            if (isTest)
                SpawnNewMap();
            else
                PickMap();
        }
    }


    private void PickMap()
    {
        if (spawnTurn % 4 != 0 || spawnTurn < 4) // 4��° ���� �ƴϸ�
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
        else // 4��° ��
        {
            if(restRange ==0) // �����۸�
            {
                spawnPosX += 19.2f;
                removeAtPosX += 19.2f;

                // Ȯ�� �����ϸ鼭 �����۴� 0~100 ���� Ư�� ���� ��ġ ���� ����
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
            else // �̾ ���� �Ѱ�
            {
                spawnPosX += 19.2f;
                removeAtPosX += 19.2f;

                Instantiate(restMapPrefab, new Vector2(spawnPosX, 0), Quaternion.identity);

                restRange = 0;
                ++spawnTurn;
            }
        }
    }

    // ���� �� �ݺ�
    private void SpawnNewMap()
    {
        spawnPosX += 19.2f;

        // ���� ť
        int nextIndex = mapIndex + 1;
        if (nextIndex >= maps.Length) nextIndex = 0;

        if(maps[nextIndex] != null) // �Ѿ ����� �ٽ� ������
        {
            maps[mapIndex] = maps[nextIndex];
            maps[nextIndex].transform.position = new Vector2(spawnPosX, 0);
        }
        else // ��� ����. �ִ� ť ũ�⸸ŭ
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
