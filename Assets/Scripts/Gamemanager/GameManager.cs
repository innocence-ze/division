using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    private void Start()
    {
        InputHandle.Instance.DisableInput();
        foreach (SpriteRenderer bd in GameObject.Find("Boards").GetComponentsInChildren<SpriteRenderer>())
        {
            bd.color = new Color(1, 1, 1, 0);
            bd.DOFade(1, 0.7f).onComplete = delegate ()
            {
                if (bd.GetComponent<EndBoard>() != null)
                {
                    foreach (SpriteRenderer c in GameObject.Find("Cells").GetComponentsInChildren<SpriteRenderer>())
                    {                       
                        c.DOFade(1, 0.7f).onComplete = delegate ()
                         {
                             InputHandle.Instance.EnableInput();
                         };
                    }
                }
            };
        }
        foreach (SpriteRenderer bd in GameObject.Find("Borders").GetComponentsInChildren<SpriteRenderer>())
        {
            bd.color = new Color(1, 1, 1, 0);
            bd.DOFade(1, 0.7f);
        }
        foreach (SpriteRenderer c in GameObject.Find("Cells").GetComponentsInChildren<SpriteRenderer>())
        {
            c.color = new Color(1, 1, 1, 0);
        }
    }

    private void Update()
    {
        if (EndPanel != null)
        {
            InputHandle.Instance.DisableInput();
        }
    }

    public void Victory()
    {
        if(!victory.activeInHierarchy)
        {
            InputHandle.Instance.DisableInput();
            victory.SetActive(true);
            victory.transform.localScale = new Vector3(0, 0, 0);
            victory.transform.DOScale(1, 1);
            victoryOrDefeatMask.SetActive(true);
        }
    }

    public void Defeat()
    {
        if(!defeat.activeInHierarchy)
        {
            InputHandle.Instance.DisableInput();
            defeat.SetActive(true);
            defeat.transform.localScale = new Vector3(0, 0, 0);
            defeat.transform.DOScale(1, 1);
            victoryOrDefeatMask.SetActive(true);
        }
    }

    public void ReturnChapter()
    {
        if(EndPanel == null)
        {
            InputHandle.Instance.EnableInput();
            Board.boards.Clear();
            SceneManager.LoadScene(PlayerPrefs.GetString("CurrentChapter"));
            return;
        }

        EndPanel.GetComponent<Image>().DOFade(0, 0.7f).onComplete = delegate ()
         {
             InputHandle.Instance.EnableInput();
             Board.boards.Clear();
             SceneManager.LoadScene(PlayerPrefs.GetString("CurrentChapter"));
         };

        foreach (var img in EndPanel.GetComponentsInChildren<Image>())
        {
            img.DOFade(0, 0.7f);
        }
    }

    public void Restart()
    {
        if(EndPanel == null)
        {
            InputHandle.Instance.EnableInput();
            Board.boards.Clear();
            SceneManager.LoadScene("Game");
            return;
        }
        EndPanel.GetComponent<Image>().DOFade(0, 0.7f).onComplete = delegate ()
        {
            InputHandle.Instance.EnableInput();
            Board.boards.Clear();
            SceneManager.LoadScene("Game");
        };

        foreach (var img in EndPanel.GetComponentsInChildren<Image>())
        {
            img.DOFade(0, 0.7f);
        }
    }

    public void NextRound()
    {
        if(EndPanel == null)
        {
            InputHandle.Instance.EnableInput();
            Board.boards.Clear();
            if (PlayerPrefs.GetString("CurrentRound") != "12")
            {
                var i = int.Parse(PlayerPrefs.GetString("CurrentRound")) + 1;
                PlayerPrefs.SetString("CurrentRound", i.ToString());
                SceneManager.LoadScene("Game");
            }
            else
                SceneManager.LoadScene(PlayerPrefs.GetString("CurrentChapter"));
            return;
        }

        EndPanel.GetComponent<Image>().DOFade(0, 0.7f).onComplete = delegate ()
        {
            InputHandle.Instance.EnableInput();
            Board.boards.Clear();
            if (PlayerPrefs.GetString("CurrentRound") != "12")
            {
                var i = int.Parse(PlayerPrefs.GetString("CurrentRound")) + 1;
                PlayerPrefs.SetString("CurrentRound", i.ToString());
                SceneManager.LoadScene("Game");
            }
            else
                SceneManager.LoadScene(PlayerPrefs.GetString("CurrentChapter"));
        };

        foreach (var img in EndPanel.GetComponentsInChildren<Image>())
        {
            img.DOFade(0, 0.7f);
        }
    }

    private GameObject EndPanel
    {
        get
        {
            if (victory.activeInHierarchy)
                return victory;
            else if (defeat.activeInHierarchy)
                return defeat;           
            return null;
        }
    }
        
}
