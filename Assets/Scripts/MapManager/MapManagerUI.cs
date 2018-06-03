using System.Collections;
using System.Collections.Generic;
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

    //当前的场景
    GameObject currentCanvas;

    //选择地板场景完成
    public void OnChangeBoardColor(string boardType)
    {
        BoardCanvas.SetActive(true);
        for(int i = 1; i < BoardCanvas.transform.childCount - 1; i++)
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
    public void OnChangeType(GameObject targetCanvas)
    {
        GetCurrentCanvas();
        currentCanvas.SetActive(false);
        targetCanvas.SetActive(true);
    }

    //获得当前的场景
    void GetCurrentCanvas()
    {
        currentCanvas = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
        while(currentCanvas.GetComponent<Canvas>() == null)
        {
            currentCanvas = currentCanvas.transform.parent.gameObject;
        }
    }

}
