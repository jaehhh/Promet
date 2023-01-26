using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private GameObject currentPopupUI;

    public void GameStartButton()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void PopupButton(GameObject target)
    {
        currentPopupUI = target;
        currentPopupUI.SetActive(true);
    }

    public void UICloseButton()
    {
        currentPopupUI.SetActive(false);
    }

    public void ExitButton()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_WINDOW
        Application.Quit();

#endif
    }
}
