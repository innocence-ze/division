using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于储存背景信息，设置锚点
public class MapManagerBackGround : MonoBehaviour {

    //四个list分别储存地板，左中右墙
    public static List<MapManagerBackGround> mapManagerBackGrounds = new List<MapManagerBackGround>();

    public GameObject cellPrefab, boardPrefab;

    public bool haveBoard = false;
    public bool haveCell = false;

    public GameObject border1;
    public GameObject border2;
    public GameObject border3;

    // Use this for initialization
    void Start () {
        mapManagerBackGrounds.Add(this);
        border1 = new GameObject("border1");
        border2 = new GameObject("border2");
        border3 = new GameObject("border3");
        border1.transform.SetParent(GameObject.Find("PrefabItems").transform);
        border2.transform.SetParent(GameObject.Find("PrefabItems").transform);
        border3.transform.SetParent(GameObject.Find("PrefabItems").transform);
        border1.tag = "MapManager";
        border2.tag = "MapManager";
        border3.tag = "MapManager";
        border1.transform.position = transform.position + new Vector3(-Board.QXX / 2f, Board.QXY / 2, 0);
        border2.transform.position = transform.position + new Vector3(Board.X / 2f, Board.Y / 2, 0);
        border3.transform.position = transform.position + new Vector3(Board.QXX / 2f, Board.QXY / 2, 0);
    }
	
    //寻找prefab，判断距离,
    public void FindPrefab(Transform prefab)
    {
        //先判断这里有没有
        if((!haveBoard && prefab.gameObject.tag == "Board") || (haveBoard && !haveCell && prefab.gameObject.tag == "Cell"))
        {
            if (Vector3.Distance(prefab.transform.position, transform.position) < 0.5f)
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
                }
                else
                {
                    sprite.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                    sprite.sortingLayerName = prefab.GetComponent<SpriteRenderer>().sortingLayerName;
                }
            }
            else
            {
                Destroy(GetComponent<SpriteRenderer>());
            }
        }
    }

    public void FindPrefab(Transform prefab, BorderDirection bd)
    {
        if(haveBoard)
        {
            switch (bd)
            {
                case BorderDirection.Down:
                case BorderDirection.Up:
                case BorderDirection.UpAndDown:
                    if (border2 != null)
                    {
                        if (Vector3.Distance(prefab.transform.position, border2.transform.position) < 0.3f)
                        {
                            if (border2.GetComponent<SpriteRenderer>() == null)
                            {
                                border2.AddComponent<SpriteRenderer>();
                            }
                            var sr = border2.GetComponent<SpriteRenderer>();
                            if (prefab.GetComponent<SpriteRenderer>() == null)
                            {
                                sr.sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                            }
                            else
                                sr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            Destroy(border2.GetComponent<SpriteRenderer>());
                        }
                    }
                    break;

                case BorderDirection.Downleft:
                case BorderDirection.Upright:
                case BorderDirection.UprightAndDownleft:
                    if (border3 != null)
                    {
                        if (Vector3.Distance(prefab.transform.position, border3.transform.position) < 0.5f)
                        {
                            if (border3.GetComponent<SpriteRenderer>() == null)
                            {
                                border3.AddComponent<SpriteRenderer>();
                            }
                            var sr = border3.GetComponent<SpriteRenderer>();
                            if (prefab.GetComponent<SpriteRenderer>() == null)
                            {
                                sr.sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                            }
                            else
                                sr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            Destroy(border3.GetComponent<SpriteRenderer>());
                        }
                    }
                    break;

                case BorderDirection.Downright:
                case BorderDirection.Upleft:
                case BorderDirection.UpleftAndDownright:
                    if (border1 != null)
                    {
                        if (Vector3.Distance(prefab.transform.position, border1.transform.position) < 0.5f)
                        {
                            if (border1.GetComponent<SpriteRenderer>() == null)
                            {
                                border1.AddComponent<SpriteRenderer>();
                            }
                            var sr = border1.GetComponent<SpriteRenderer>();
                            if (prefab.GetComponent<SpriteRenderer>() == null)
                            {
                                sr.sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;
                            }
                            else
                                sr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            Destroy(border1.GetComponent<SpriteRenderer>());
                        }
                    }
                    break;
            }
        }
    }

    //让prefab的position等于自己的position
    public bool FixedPrefabPosition(Transform prefab)
    {
        if(Vector3.Distance(prefab.position,transform.position) < 0.5f && ( (prefab.gameObject.tag == "Cell" && !haveCell && haveBoard) || (prefab.gameObject.tag == "Board" && !haveBoard) ))
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

    public bool FixedPrefabPosition(Transform prefab, BorderDirection bd)
    {
        if(haveBoard)
        {
            switch (bd)
            {
                case BorderDirection.Down:
                case BorderDirection.Up:
                case BorderDirection.UpAndDown:
                    if (border2 != null)
                    {
                        if (Vector3.Distance(prefab.position, border2.transform.position) < 0.5f)
                        {
                            prefab.position = border2.transform.position;
                            Destroy(border2.gameObject);
                            return true;
                        }
                    }
                    break;

                case BorderDirection.Downleft:
                case BorderDirection.Upright:
                case BorderDirection.UprightAndDownleft:
                    if (border3 != null)
                    {
                        if (Vector3.Distance(prefab.position, border3.transform.position) < 0.5f)
                        {
                            prefab.position = border3.transform.position;
                            Destroy(border3.gameObject);
                            return true;
                        }
                    }
                    break;

                case BorderDirection.Downright:
                case BorderDirection.Upleft:
                case BorderDirection.UpleftAndDownright:
                    if (border1 != null)
                    {
                        if (Vector3.Distance(prefab.position, border1.transform.position) < 0.5f)
                        {
                            prefab.position = border1.transform.position;
                            Destroy(border1.gameObject);
                            return true;
                        }
                    }
                    break;
            }
        }
        return false;
    }



}
