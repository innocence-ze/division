using UnityEngine;

//用于移动删除背景上的物体
public class MapManagerPrefab : MonoBehaviour
{
    private Vector3 mousePosition;

    private void DeletePrefab()
    {
        if(Input.GetMouseButtonUp(0))
        {
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (mb.haveBoard && Vector3.Distance(mousePosition, mb.transform.position) <= 0.3f)
                {
                    if (mb.haveCell)
                    {
                        if (mb.cellPrefab.name == "Coin")
                        {
                            MapManager.instance.HaveCoin = false;
                        }
                        if (mb.cellPrefab.name == "Cell")
                        {
                            MapManager.instance.CellCount -= 1;
                        }
                        Destroy(mb.cellPrefab);
                        mb.haveCell = false;
                        return;
                    }
                    else
                    {

                        if (mb.boardPrefab.name == "EndBoard")
                        {
                            MapManager.instance.HaveEndboard = false;
                        }
                        Board.boards.Remove(mb.boardPrefab.GetComponent<Board>());
                        Destroy(mb.boardPrefab);
                        mb.haveBoard = false;
                        return;
                    }
                }
            }
            foreach (MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
            {
                if (mb.haveBorder && Vector3.Distance(mousePosition, mb.transform.position) <= 0.2f)
                {
                    if (mb.haveBorder)
                    {
                        Destroy(mb.borderPrefab);
                        mb.haveBorder = false;
                    }
                }
            }
        }
    }

    private GameObject movePrefab;
    private Vector3 position;
    private bool havePrefab = false;

    private void MovePrefab()
    {
        //找到要移动的物体
        if (Input.GetMouseButtonDown(0))
        {
            if (!havePrefab)
            {
                foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
                {
                    if (mb.haveBoard && Vector3.Distance(mousePosition, mb.transform.position) <= 0.4f)
                    {
                        havePrefab = true;
                        movePrefab = mb.haveCell ? mb.cellPrefab : mb.boardPrefab;
                        position = mb.transform.position;

                        if (mb.haveCell)
                        {
                            mb.cellPrefab = null;
                            mb.haveCell = false;
                        }
                        else
                        {
                            mb.boardPrefab = null;
                            mb.haveBoard = false;
                        }
                        movePrefab.GetComponent<SpriteRenderer>().sortingOrder += 1;
                        break;
                    }
                }
            }
 
            if (!havePrefab)
            {
                foreach(MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
                {
                    if(mb.haveBorder && Vector3.Distance(mousePosition, mb.transform.position) <= 0.2f)
                    {
                        havePrefab = true;
                        movePrefab = mb.borderPrefab;
                        position = mb.transform.position;
                        mb.borderPrefab = null;
                        mb.haveBorder = false;
                        movePrefab.GetComponent<SpriteRenderer>().sortingOrder += 1;
                        break;
                    }
                }
            }
            
        }
        if (havePrefab)
        {
            OnMovePrefab(movePrefab, movePrefab.tag);
        }
        if(Input.GetMouseButtonUp(0) && havePrefab)
        {
            OnEndMove(movePrefab,position,movePrefab.tag);
            havePrefab = false;
        }
    }

    //prefab移动的物体
    private void OnMovePrefab(GameObject prefab, string tag)
    {
        prefab.transform.position = mousePosition;
        if(!tag.Equals("Border"))
        {
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (Vector3.Distance(mb.transform.position, prefab.transform.position) < 0.4f && ((!mb.haveBoard && tag == "Board") || (mb.haveBoard && !mb.haveCell && tag == "Cell")))
                {
                    if (mb.GetComponent<SpriteRenderer>() == null)
                    {
                        var sr = mb.gameObject.AddComponent<SpriteRenderer>();
                        sr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                        sr.sortingLayerName = prefab.GetComponent<SpriteRenderer>().sortingLayerName;
                        sr.sortingOrder = prefab.GetComponent<SpriteRenderer>().sortingOrder - 1;
                    }
                }
                else if (mb.GetComponent<SpriteRenderer>() != null)
                {
                    Destroy(mb.GetComponent<SpriteRenderer>());
                }
            }
        }
        else
        {
            foreach (MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
            {
                if (Vector3.Distance(mousePosition, mb.transform.position) <= 0.2f && !mb.haveBorder)
                {
                    prefab.transform.rotation = mb.transform.rotation;
                    if (mb.GetComponent<SpriteRenderer>() == null)
                    {
                        var sr = mb.gameObject.AddComponent<SpriteRenderer>();
                        sr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                        sr.sortingLayerName = prefab.GetComponent<SpriteRenderer>().sortingLayerName;
                        sr.sortingOrder = prefab.GetComponent<SpriteRenderer>().sortingOrder - 1;
                    }
                }
                else if (mb.GetComponent<SpriteRenderer>() != null)
                {
                    Destroy(mb.GetComponent<SpriteRenderer>());
                }
            }
        }       
    }

    private void OnEndMove(GameObject prefab, Vector3 position, string tag)
    {
        bool isOnBG = false;
        prefab.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        if (tag != "Border")
        {
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (Vector3.Distance(mb.transform.position, prefab.transform.position) < 0.4f)
                {
                    if (mb.GetComponent<SpriteRenderer>() != null)
                    {
                        Destroy(mb.GetComponent<SpriteRenderer>());
                    }
                    if (!mb.haveBoard && prefab.tag == "Board")
                    {
                        mb.haveBoard = true;
                        mb.boardPrefab = prefab;
                        prefab.transform.position = mb.transform.position;
                        isOnBG = true;
                    }
                    if (mb.haveBoard && !mb.haveCell && prefab.tag == "Cell")
                    {
                        mb.haveCell = true;
                        mb.cellPrefab = prefab;
                        prefab.transform.position = mb.transform.position;
                        isOnBG = true;
                    }
                    break;
                }
            }
        }
        else
        {
            foreach (MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
            {
                if (Vector3.Distance(mb.transform.position, prefab.transform.position) < 0.2f)
                {
                    mb.SetEularAngleZ(prefab);
                    if (mb.GetComponent<SpriteRenderer>() != null)
                    {
                        Destroy(mb.GetComponent<SpriteRenderer>());
                    }
                    if (!mb.haveBorder)
                    {
                        mb.haveBorder = true;
                        mb.borderPrefab = prefab;
                        prefab.transform.position = mb.transform.position;
                        isOnBG = true;
                    }
                    break;
                }
            }
        }
        if (!isOnBG)
        {
            prefab.transform.position = position;
            if (tag != "Border")
            {
                foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
                {
                    if (Vector3.Distance(mb.transform.position, prefab.transform.position) < 0.3f)
                    {
                        if (prefab.tag == "Cell")
                        {
                            mb.haveCell = true;
                            mb.cellPrefab = prefab;
                            break;
                        }
                        else if (prefab.tag == "Board")
                        {
                            mb.haveBoard = true;
                            mb.boardPrefab = prefab;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
                {
                    if (Vector3.Distance(mb.transform.position, prefab.transform.position) < 0.2f)
                    {
                        mb.haveBorder = true;
                        mb.borderPrefab = prefab;
                        break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
        if(MapManagerUI.instance.Tog.isOn)
        {
            DeletePrefab();
        }
        else
        {
            MovePrefab();
        }
    }
}
