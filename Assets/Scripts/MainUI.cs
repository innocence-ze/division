using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MainUI : MonoBehaviour {

    public GameObject sound;
    private GameObject existSound = null;
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
    [SerializeField]
    [Header("【标题】")]
    private GameObject title;
    [SerializeField]
    [Header("【遮罩】")]
    private Image mask;

    GameObject[] gos;

    private void Start()
    {
        gos = new GameObject[5]
        {
            title,
            beginGame,
            continueGame,
            map,
            quitGame            
        };
        int i = 0;
        foreach(GameObject go in gos)
        {        
            go.GetComponent<Image>().DOFade(1, 0.5f + 0.4f * i).onComplete = delegate(){ go.GetComponent<Image>().raycastTarget = true; };
            i++;
        }
        
        existSound = GameObject.FindGameObjectWithTag("Sound");
        if(existSound == null)
        {
            existSound = Instantiate(sound);
        }
        DontDestroyOnLoad(existSound);
    }

    public void QuitGame()
    {
        mask.DOFade(1, 0.6f).onComplete = delegate ()
         {
             Application.Quit();
         };

    }

    public void BeginGame()
    {
        mask.DOFade(1, 0.6f).onComplete = delegate ()
        {
            SceneManager.LoadScene("1");
        };
        
    }

    public void ContinueGame()
    {
        mask.DOFade(1, 0.6f).onComplete = delegate ()
        {
            SceneManager.LoadScene("1");
        };
    
    }

    public void Map()
    {
        mask.DOFade(1, 0.6f).onComplete = delegate ()
        {
            SceneManager.LoadScene("MapManager"); 
        };
    }

}
