using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSomething : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private GameObject flyMonster;
    [SerializeField]
    private GameObject healpack;
    [SerializeField]
    private GameObject hitObstacle;
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private GameObject pillarObstacle;
    [SerializeField]
    private GameObject handspike;
    [SerializeField]
    private GameObject itemBox;

    public void Awake()
    {
        player = GameObject.Find("Person").transform;
    }

    public void SpawnEnemyWithTestButton()
    {
        Instantiate(flyMonster);
    }

    public void SpawnHealpack()
    {
        Instantiate(healpack, player.transform.position + Vector3.right * 18, Quaternion.identity);
    }

    public void SpawnHitObstacle()
    {
        Instantiate(hitObstacle, player.transform.position + Vector3.right * 18, Quaternion.identity);
    }


    public void SpawnPlatform()
    {
        Instantiate(platform, player.transform.position + Vector3.right * 25 + Vector3.up * 2, Quaternion.identity);
    }

    public void SpawnPillarObstacle()
    {
        Instantiate(pillarObstacle, new Vector2(player.transform.position.x + 18, -2.7f), Quaternion.identity);
    }

    public void SpawnHandspike()
    {
        Instantiate(handspike, player.transform.position + Vector3.right * 18, Quaternion.identity);
    }

    public void SpawnItemBox()
    {
        Instantiate(itemBox, player.transform.position + Vector3.right * 18, Quaternion.identity);
    }
}
