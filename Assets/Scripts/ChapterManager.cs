using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ChapterManager : MonoBehaviour {

    private void Start()
    {
        PlayerPrefs.SetString("CurrentChapter", SceneManager.GetActiveScene().name);
    }

    public void OnChooseRound()
    {
        string round = EventSystem.current.currentSelectedGameObject.name;
        PlayerPrefs.SetString("CurrentRound", round);
        SceneManager.LoadScene("Game");
    }

    public void OnReturnMain()
    {
        SceneManager.LoadScene("Main");
    }
}
