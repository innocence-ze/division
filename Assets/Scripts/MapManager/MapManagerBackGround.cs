using System.Collections.Generic;
using UnityEngine;

//用于储存背景信息，设置锚点
public class MapManagerBackGround : MonoBehaviour {

    //list储存地板  数组储存细胞壁
    public static List<MapManagerBackGround> mapManagerBackGrounds = new List<MapManagerBackGround>();

    public GameObject cellPrefab, boardPrefab;

    public bool haveBoard = false;
    public bool haveCell = false;

    void Awake () {
        mapManagerBackGrounds.Add(this);        
    }

    //Prefab与当前锚点，并在锚点添加图片
    public void FindPrefab(Transform prefab)
    {
        //先判断这里有没有prefab
        if((!haveBoard && prefab.gameObject.tag == "Board") || (haveBoard && !haveCell && prefab.gameObject.tag == "Cell"))
        {
            //prefab靠过来后增加图片
            if (gameObject.GetComponent<SpriteRenderer>() == null)
            {
                gameObject.AddComponent<SpriteRenderer>();
            }
            var sprite = gameObject.GetComponent<SpriteRenderer>();
            if (prefab.GetComponent<SpriteRenderer>() == null)
            {
                sprite.sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                sprite.sortingLayerName = prefab.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
                sprite.sortingOrder = prefab.GetComponentInChildren<SpriteRenderer>().sortingOrder - 1;
            }
            else
            {
                sprite.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                sprite.sortingLayerName = prefab.GetComponent<SpriteRenderer>().sortingLayerName;
                sprite.sortingOrder = prefab.GetComponent<SpriteRenderer>().sortingOrder - 1;
            }
        }       
    }
 
    //让prefab固定在锚点上
    public bool FixedPrefabPosition(Transform prefab)
    {
        if((prefab.gameObject.tag == "Cell" && !haveCell && haveBoard) || (prefab.gameObject.tag == "Board" && !haveBoard))
        {
            if (prefab.gameObject.tag == "Cell")
            {
                haveCell = true;
                cellPrefab = prefab.gameObject;
            }
            else
            {
                haveBoard = true;
                boardPrefab = prefab.gameObject;
            }
            prefab.position = transform.position;        
            Destroy(GetComponent<SpriteRenderer>());
            return true;
        }        
        return false;
    }

}
