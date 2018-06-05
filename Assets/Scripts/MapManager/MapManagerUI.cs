using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

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
        background.GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/Background/" + backgroundType);
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
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        var sprite = Resources.Load<Sprite>("MapManager/MapManagerBackground");
        GameObject background = GameObject.Find("BackGround");
        background.GetComponent<Image>().sprite = sprite;
        MainCanvas.SetActive(true);
    }

    //TODO后台添加
    public void OnPlay()
    {
        GetCurrentCanvas();
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
        if(currentButton.name == "Edit")
        {
            currentButton = currentButton.transform.parent.gameObject;
        }
        fileName = currentButton.name;
        for(int i = 0; i < 3; i++)
        {
            currentButton.transform.GetChild(i).gameObject.SetActive(true);
        }
        currentButton.GetComponent<Button>().enabled = false;
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        BackGroundCanvas.SetActive(true);
    }

    //TODO后台添加
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
