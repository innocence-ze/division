using System.Collections.Generic;
using UnityEngine;

//用于储存背景信息，设置锚点
public class MapManagerBackGround : MonoBehaviour {

    //list储存地板  数组储存细胞壁
    public static List<MapManagerBackGround> mapManagerBackGrounds = new List<MapManagerBackGround>();
    public GameObject[] borders;

    public GameObject cellPrefab, boardPrefab;

    public bool haveBoard = false;
    public bool haveCell = false;
    


    // Use this for initialization
    void Start () {
        mapManagerBackGrounds.Add(this);

        borders = new GameObject[6];
        for(int i = 0; i < 6; i++)
        {
            borders[i] = new GameObject("border" + i);
            borders[i].transform.SetParent(GameObject.Find("PrefabItems").transform);
            borders[i].tag = "MapManager";
        }
        borders[0].transform.position = transform.position + new Vector3(Board.QXX / 2f, Board.QXY / 2, 0);
        borders[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60));
        borders[1].transform.position = transform.position + new Vector3(Board.X / 2f, Board.Y / 2, 0);
        borders[1].transform.rotation = new Quaternion(0, 0, 0, 1);
        borders[2].transform.position = transform.position + new Vector3(-Board.QXX / 2f, Board.QXY / 2, 0);
        borders[2].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 60));
        borders[3].transform.position = transform.position + new Vector3(-Board.QXX / 2f, -Board.QXY / 2, 0);
        borders[3].transform.rotation = Quaternion.Euler(new Vector3(0, 0, -60));
        borders[4].transform.position = transform.position + new Vector3(Board.X / 2f, -Board.Y / 2, 0);
        borders[4].transform.rotation = new Quaternion(0, 0, 0, 1);
        borders[5].transform.position = transform.position + new Vector3(Board.QXX / 2f, -Board.QXY / 2, 0);
        borders[5].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 60));
    }

    int borderNumber;
    GameObject border;
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
        if(haveBoard && prefab.tag == "Border")
        {
            float distance = 2;
            int i = 0;
            //寻找最近的border
            foreach(GameObject b in borders)
            {            
                if (Vector3.Distance(b.transform.position, prefab.position) < distance)
                {
                    distance = Vector3.Distance(b.transform.position, prefab.position);
                    border = b;
                    borderNumber = i;
                }
                i++;
            }
            i = 0;
            //删除其它border的图片
            foreach(GameObject b in borders)
            {
                if(i != borderNumber)
                {
                    Destroy(b.GetComponent<SpriteRenderer>());
                }
                i++;
            }
            if(border.GetComponent<SpriteRenderer>() == null)
            {
                border.AddComponent<SpriteRenderer>();
            }
            var sprite = border.GetComponent<SpriteRenderer>();
            sprite.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            sprite.sortingLayerName = prefab.GetComponent<SpriteRenderer>().sortingLayerName;
            sprite.sortingOrder = prefab.GetComponent<SpriteRenderer>().sortingOrder - 1;
            switch (borderNumber)
            {
                case 0:
                case 3:
                    prefab.rotation = Quaternion.Euler(new Vector3(0, 0, -60));
                    break;
                case 1:
                case 4:
                    prefab.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case 2:
                case 5:
                    prefab.rotation = Quaternion.Euler(new Vector3(0, 0, 60));
                    break;
            }

        }
    }
 
    //TODO
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
        else if(prefab.gameObject.tag == "Border")
        {
            prefab.position = border.transform.position;
            prefab.rotation = border.transform.rotation;
            if(prefab.name == "Up")
            {
               

            }
            return true;
        }
        return false;
    }

    //TODO 可能不要
    //让prefab固定在锚点上，prefab类型为border
    public bool FixedPrefabPosition(Transform prefab, BorderDirection bd)
    {
        //if(haveBoard)
        //{
        //    switch (bd)
        //    {
        //        case BorderDirection.Down:
        //        case BorderDirection.Up:
        //        case BorderDirection.UpAndDown:
        //            if (border2 != null)
        //            {
        //                if (Vector3.Distance(prefab.position, border2.transform.position) < 0.5f)
        //                {
        //                    prefab.position = border2.transform.position;
        //                    Destroy(border2.gameObject);
        //                    return true;
        //                }
        //            }
        //            break;

        //        case BorderDirection.Downleft:
        //        case BorderDirection.Upright:
        //        case BorderDirection.UprightAndDownleft:
        //            if (border3 != null)
        //            {
        //                if (Vector3.Distance(prefab.position, border3.transform.position) < 0.5f)
        //                {
        //                    prefab.position = border3.transform.position;
        //                    Destroy(border3.gameObject);
        //                    return true;
        //                }
        //            }
        //            break;
        //        case BorderDirection.Downright:
        //        case BorderDirection.Upleft:
        //        case BorderDirection.UpleftAndDownright:
        //            if (border1 != null)
        //            {
        //                if (Vector3.Distance(prefab.position, border1.transform.position) < 0.5f)
        //                {
        //                    prefab.position = border1.transform.position;
        //                    Destroy(border1.gameObject);
        //                    return true;
        //                }
        //            }
        //            break;
        //    }
        //}
        return false;
    }

}
