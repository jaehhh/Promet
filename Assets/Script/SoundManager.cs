using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource buttonClickAudio;
    private AudioSource bgmAudio;

    [SerializeField]
    private AudioClip bgmTitle;
    [SerializeField]
    private AudioClip[] bgmMainGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        buttonClickAudio = transform.GetChild(0).GetComponent<AudioSource>();
        bgmAudio = transform.GetChild(1).GetComponent<AudioSource>();

        SceneManager.sceneLoaded += BGMChoice;
    }

    public void ButtonClickSound()
    {
        buttonClickAudio.Play();
    }

    private void BGMChoice(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Title")
        {
            bgmAudio.clip = bgmTitle;
            bgmAudio.Play();
        }
        else
        {
            int random = Random.Range(0, bgmMainGame.Length);
            bgmAudio.clip = bgmMainGame[random];
            bgmAudio.Play();
        }
    }
}
