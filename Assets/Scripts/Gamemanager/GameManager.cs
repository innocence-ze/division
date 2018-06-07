using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [HideInInspector]public static GameManager instance;

    [HideInInspector] public List<Direction> stepList = new List<Direction>();

    public string sceneName;
    public string nextSceneName;

    [SerializeField] private GameObject defeat;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject victoryOrDefeatMask;

    // Use this for initialization
    void Awake () {
        instance = this;
    }

    private void Update()
    {
        print(Board.boards.Count());
    }

    public void Victory()
    {
        if(!victory.activeInHierarchy)
        {
            victory.SetActive(true);
            victoryOrDefeatMask.SetActive(true);
        }
    }

    public void Defeat()
    {
        if(!defeat.activeInHierarchy)
        {
            defeat.SetActive(true);
            victoryOrDefeatMask.SetActive(true);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        Board.boards.Clear();
        SceneManager.LoadScene(sceneName);
    }

    public void NextScene()
    {
        Board.boards.Clear();
        SceneManager.LoadScene(nextSceneName);
    }
}
