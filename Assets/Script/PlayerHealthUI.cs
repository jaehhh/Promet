using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    private GameObject[] points;

    // ������
    [SerializeField]
    private GameObject healthPointPrefab;
    [SerializeField]
    private Sprite onHealth;
    [SerializeField]
    private Sprite offHealth;

    private int currentHealth;

    private void Awake()
    {
        MoveController player = GameObject.Find("Person").GetComponent<MoveController>();
        player.playerHealthUI = this;

        points = new GameObject[player.maxHealth];
        currentHealth = points.Length;
        SetupHealthPoint();
    }

    private void SetupHealthPoint()
    {
        for(int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i).gameObject;
        }
    }

    public void DecreaseHealth()
    {
        if (currentHealth > 0)
            points[--currentHealth].GetComponent<Image>().enabled = false;
    }

    public void IncreaseHealth()
    {
        if (currentHealth < points.Length)
            points[currentHealth++].GetComponent<Image>().enabled = true;
    }
}
