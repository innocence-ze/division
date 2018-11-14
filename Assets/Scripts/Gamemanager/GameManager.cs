using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [HideInInspector] public List<Direction> stepList = new List<Direction>();

    [SerializeField] private GameObject defeat;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject victoryOrDefeatMask;

    // Use this for initialization
    void Awake () {
        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            print(Board.boards.Count);
        }
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

    public void ReturnChapter()
    {
        Board.boards.Clear();
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentChapter"));
    }

    public void Restart()
    {
        Board.boards.Clear();
        SceneManager.LoadScene("Game");
    }

    public void NextRound()
    {
        Board.boards.Clear();
        if (PlayerPrefs.GetString("CurrentRound")!="12")
        {
            var i = int.Parse(PlayerPrefs.GetString("CurrentRound")) + 1;
            PlayerPrefs.SetString("CurrentRound", i.ToString());
            SceneManager.LoadScene("Game");
        }
        else
            SceneManager.LoadScene(PlayerPrefs.GetString("CurrentChapter"));

    }
}
