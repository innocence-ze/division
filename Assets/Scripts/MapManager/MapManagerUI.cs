﻿using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MapManagerUI : MonoBehaviour {

    public static MapManagerUI instance;

    [SerializeField]
    [Header("【选择地板的UI】")]
    private GameObject BoardCanvas;
    [SerializeField]
    [Header("【选择地板颜色的UI】")]
    private GameObject BoardColorCanvas;
    [SerializeField]
    [Header("【选择细胞的UI】")]
    private GameObject CellCanvas;
    [SerializeField]
    [Header("【选择边界的UI】")]
    private GameObject BorderCanvas;
    [SerializeField]
    [Header("【选择类型的UI】")]
    private GameObject ChoseTypeCanvas;
    [SerializeField]
    [Header("【选择背景的UI】")]
    private GameObject BackGroundCanvas;
    [SerializeField]
    [Header("【主界面的UI】")]
    private GameObject MainCanvas;
    [SerializeField]
    [Header("【试玩界面的UI】")]
    private GameObject PlayCanvas;
    [SerializeField]
    [Header("【胜利失败界面的UI】")]
    private GameObject End;

    private void Start()
    {
        instance = this;
        for (int i = 1; i <= 6; i++)
        {
            string xmlPath = Application.dataPath + "/Data/" + i.ToString() + ".png";
            if (File.Exists(xmlPath))
            {
                Sprite s = MapManager.instance.ReadPNG(i.ToString());
                Transform t = MainCanvas.transform.GetChild(i);
                t.GetComponent<Image>().sprite = s;
                t.GetComponent<Button>().enabled = false;
                for(int j = 0; j < t.childCount; j++)
                {
                    t.GetChild(j).gameObject.SetActive(true);
                }
            }
        }
    }

    //当前的场景
    GameObject currentCanvas;
    GameObject currentButton;
    string fileName;

    public void Victory()
    {
        End.SetActive(true);
        End.transform.Find("Mask").gameObject.SetActive(true);
        End.transform.Find("Victory").gameObject.SetActive(true);
    }

    public void Defeat()
    {
        End.SetActive(true);
        End.transform.Find("Mask").gameObject.SetActive(true);
        End.transform.Find("Defeat").gameObject.SetActive(true);
    }

    public void OnReplay()
    {

    }

    public void OnReturnMenu()
    {
        MapManagerBackGround.mapManagerBackGrounds.Clear();
        SceneManager.LoadScene("Main");
    }

    //选择地板场景完成
    public void OnChangeBoardColor(string boardType)
    {
        BoardCanvas.SetActive(true);
        for(int i = 1; i < 6; i++)
        {
            BoardCanvas.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/Board/" + boardType + "." + i); 
        }
        BoardColorCanvas.SetActive(false);

    }

    //选择背景完成
    public void OnChangeBackground(string backgroundType)
    {
        BackGroundCanvas.SetActive(false);
        GameObject background = GameObject.Find("BackGround");
        GameObject shotBackground = GameObject.Find("BackGroundShot");
        background.GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/Background/" + backgroundType);
        shotBackground.GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/Background/" + backgroundType);
        ChoseTypeCanvas.SetActive(true);
    }

    //场景切换时（选择细胞边界等只改变底部状态栏的场景）
    public void OnChangeCanvas(GameObject targetCanvas)
    {
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        targetCanvas.SetActive(true);
    }

    public void OnReturnMain()
    {

        GetCurrentCanvas();
        if(currentCanvas.name == "Play" ||currentCanvas.name == "End")
        {
            GameObject mm = GameObject.Find("MapManager");
            mm.AddComponent<MapManager>();
            mm.AddComponent<MapManagerPrefab>();
            Destroy(GameObject.Find("GameManager"));
        }
        MapManager.instance.gamePrefabs.Clear();
        if (currentCanvas.name != "Play" && currentCanvas.name != "End")
        {
            MapManager.instance.SaveXML(fileName);
            MapManager.instance.SavePNG(fileName);
        }

        MapManager.instance.Init("Cell");
        MapManager.instance.Init("Board");
        MapManager.instance.Init("Border");
        foreach(MapManagerBackGround mmbg in MapManagerBackGround.mapManagerBackGrounds)
        {
            mmbg.haveBoard = false;
            mmbg.haveCell = false;
        }
        GetCurrentCanvas();
        currentCanvas.SetActive(false);

        var sprite = Resources.Load<Sprite>("MapManager/MapManagerBackground");
        GameObject background = GameObject.Find("BackGround");
        GameObject shotBackground = GameObject.Find("BackGroundShot");
        background.GetComponent<Image>().sprite = sprite;
        shotBackground.GetComponent<Image>().sprite = sprite;

        MainCanvas.SetActive(true);
        if (currentCanvas.name != "Play" && currentCanvas.name != "End")
            currentButton.GetComponent<Image>().sprite = MapManager.instance.ReadPNG(currentButton.name);

    }

    public void OnPlay()
    {
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        if (currentCanvas.name != "End")
        {
            //获取文件名字
            if (currentCanvas.name == "Main")
            {
                fileName = EventSystem.current.currentSelectedGameObject.transform.parent.name;
            }
            if (currentCanvas.name != "Main")
            {
                MapManager.instance.SaveXML(fileName);
                MapManager.instance.SavePNG(fileName);
            }
            GameObject g = Resources.Load<GameObject>("GameManager");
            g = Instantiate(g);
            g.name = "GameManager";
        }       
        MapManager.instance.LoadXML(fileName);
        if(MapManager.instance.IsRightMap())
        {
            PlayCanvas.SetActive(true);
            GameObject mm = GameObject.Find("MapManager");
            Destroy(mm.GetComponent<MapManager>());
            Destroy(mm.GetComponent<MapManagerPrefab>());
        }
        else
        {
            ChoseTypeCanvas.SetActive(true);
        }
    }

    public void OnBeginEdit()
    {
        //设置文件名字
        currentButton = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < 3; i++)
        {
            currentButton.transform.GetChild(i).gameObject.SetActive(true);
        }
        currentButton.GetComponent<Button>().enabled = false;
        fileName = currentButton.name;
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        BackGroundCanvas.SetActive(true);
    }

    public void OnContinueEdit()
    {
        currentButton = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        fileName = currentButton.name;
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        ChoseTypeCanvas.SetActive(true);
        MapManager.instance.ReadXML(fileName);
    }

    public void DelMap()
    {
        currentButton = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        fileName = currentButton.name;
        currentButton.GetComponent<Button>().enabled = true;
        for (int i = 0; i < 3; i++)
        {
            currentButton.transform.GetChild(i).gameObject.SetActive(false);
        }
        currentButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/AddMap");

        if(File.Exists(Application.dataPath + "/Data/" + fileName + ".png"))
        {
            File.Delete(Application.dataPath + "/Data/" + fileName + ".png");
        }
        if (File.Exists(Application.dataPath + "/Data/" + fileName + ".png.meta"))
        {
            File.Delete(Application.dataPath + "/Data/" + fileName + ".png.meta");
        }
        if (File.Exists(Application.dataPath + "/Data/" + fileName + ".xml"))
        {
            File.Delete(Application.dataPath + "/Data/" + fileName + ".xml");
        }
        if (File.Exists(Application.dataPath + "/Data/" + fileName + ".xml.meta"))
        {
            File.Delete(Application.dataPath + "/Data/" + fileName + ".xml.meta");
        }
    }

    //获得当前的UI画布
    void GetCurrentCanvas()
    {
        currentCanvas = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        while(currentCanvas.GetComponent<Canvas>() == null)
        {
            currentCanvas = currentCanvas.transform.parent.gameObject;
        }
    }

}
