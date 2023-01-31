using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameStartUI;
    [SerializeField]
    private GameObject gameOverUI;
    private TextMeshProUGUI gameOverText;

    public bool isGameRunning;
    private float currentTime;
    private int currentTimerTime;

    private void Start()
    {
        GameStart();
    }

    private void Update()
    {
        if (!isGameRunning) return;
        currentTime += Time.deltaTime;
    }

    public void GameStart()
    {
        gameStartUI.SetActive(true);

        Invoke("Timer", 1f);
    }

    private void Timer()
    {
        if(currentTimerTime == 0) // 타이머 첫 시작
        {
            currentTimerTime = 3;
        }

        --currentTimerTime;

        gameStartUI.GetComponentInChildren<TextMeshProUGUI>().text = currentTimerTime.ToString();


        if (currentTimerTime == 0)
        {
            isGameRunning = true;
            GameObject.Find("Person").GetComponent<MoveController>().isGameRunning = true;

            gameStartUI.SetActive(false);
        }
        else
        {
            Invoke("Timer", 1f);
        }
    }

    public void GameOver()
    {
        isGameRunning = false;
        GameObject.Find("Person").GetComponent<MoveController>().isGameRunning = false;

        int min = (int)currentTime / 60;
        int sec = (int) currentTime % 60;

        gameOverUI.SetActive(true);
        gameOverText = gameOverUI.GetComponentInChildren<TextMeshProUGUI>();
        gameOverText.text = $"GameOver...\n버틴 시간: {min}분 {sec}초";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void ButtonSound()
    {
        SoundManager.instance.ButtonClickSound();
    }
}
