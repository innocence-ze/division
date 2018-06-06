using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MapManagerUI : MonoBehaviour {
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

    //当前的场景
    GameObject currentCanvas;
    GameObject currentButton;
    string fileName;

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

    //TODO后台添加
    public void OnReturnMain()
    {
        MapManager.instance.SaveXML(fileName);
        MapManager.instance.SavePNG(fileName);
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        var sprite = Resources.Load<Sprite>("MapManager/MapManagerBackground");
        GameObject background = GameObject.Find("BackGround");
        GameObject shotBackground = GameObject.Find("BackGroundShot");
        background.GetComponent<Image>().sprite = sprite;
        shotBackground.GetComponent<Image>().sprite = sprite;
        MainCanvas.SetActive(true);       
        currentButton.GetComponent<Image>().sprite = MapManager.instance.ReadPNG(currentButton.name);

    }

    //TODO后台添加
    public void OnPlay()
    {
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        PlayCanvas.SetActive(true);
        //获取文件名字
        if(currentCanvas.name == "Main")
        {
            fileName = EventSystem.current.currentSelectedGameObject.transform.parent.name;
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

    //文件读取
    public void OnContinueEdit()
    {
        currentButton = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        fileName = currentButton.name;
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        ChoseTypeCanvas.SetActive(true);
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
        if(File.Exists(Application.dataPath + "/Data/" + fileName + ".xml"))
        {
            File.Delete(Application.dataPath + "/Data/" + fileName + ".xml");
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
