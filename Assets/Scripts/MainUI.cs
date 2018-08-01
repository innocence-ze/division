using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    [SerializeField]
    [Header("【开始】")]
    private GameObject beginGame;
    [SerializeField]
    [Header("【继续】")]
    private GameObject continueGame;
    [SerializeField]
    [Header("【地图】")]
    private GameObject map;
    [SerializeField]
    [Header("【退出】")]
    private GameObject quitGame;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BeginGame()
    {
        SceneManager.LoadScene("1");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("1");
    }

    public void Map()
    {
        SceneManager.LoadScene("MapManager"); 
    }
}
