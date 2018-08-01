using UnityEngine;

//用于移动删除背景上的物体
public class MapManagerPrefab : MonoBehaviour
{
    private Vector3 mousePosition;

    //TODO
    void DeletePrefab()
    {
        if(Input.GetMouseButtonUp(1))
        {           
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if(Vector3.Distance(mousePosition,mb.transform.position) <= 0.6f)
                {
                    //删除细胞壁
                    if (Vector3.Distance(mousePosition, mb.transform.position) >= 0.4f && mousePosition.y > transform.position.y)
                    {
                        //if(mousePosition.x - transform.position.x < -0.5f && mb.border1 != null)
                        //{
                        //    Destroy(mb.border1);
                        //    break;
                        //}
                        //else if(mousePosition.x - transform.position.x > -0.5f && mousePosition.x - transform.position.x < 0.5f && mb.border2 != null)
                        //{
                        //    Destroy(mb.border2);
                        //    break;
                        //}
                        //else if(mousePosition.x - transform.position.x > 0.5f && mb.border3 != null)
                        //{
                        //    Destroy(mb.border3);
                        //    break;
                        //}
                    }
                    else if (mb.haveBoard && Vector3.Distance(mousePosition, mb.transform.position) <= 0.4f)
                    {
                        if(mb.haveCell)
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
                            break;
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
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
        }
    }

    //是否有要移动的物体
    private bool havePrefab = false;
    private GameObject movePrefab;
    private Vector3 position;

    //TODO
    //添加细胞壁
    void MovePrefab()
    {
        //找到要移动的物体
        if (Input.GetMouseButtonDown(0))
        {
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (Vector3.Distance(mousePosition, mb.transform.position) <= 0.6f)
                {
                    if (Vector3.Distance(mousePosition, mb.transform.position) >= 0.4f && mousePosition.y > transform.position.y)
                    {

                    }
                    if (mb.haveBoard && Vector3.Distance(mousePosition, mb.transform.position) <= 0.4f)
                    {
                        havePrefab = true;
                        movePrefab = mb.haveCell ? mb.cellPrefab : mb.boardPrefab;
                        position = mb.transform.position;
                        
                        if(mb.haveCell)
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
                    }
                }
            }
        }
        if (havePrefab)
        {
            MovePrefab(movePrefab);
        }
        if(Input.GetMouseButtonUp(0) && havePrefab)
        {
            OnEndMove(movePrefab,position);
            havePrefab = false;
        }
    }

    //prefab移动的物体
    //TODO
    //添加细胞壁
    void MovePrefab(GameObject prefab)
    {
        prefab.transform.position = mousePosition;
        foreach(MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
        {
            if(((!mb.haveBoard &&prefab.tag == "Board") || (mb.haveBoard && !mb.haveCell && prefab.tag == "Cell"))&& Vector3.Distance(mb.transform.position,prefab.transform.position) < 0.5f)
            {
                if(mb.GetComponent<SpriteRenderer>() == null)
                {
                    var sr = mb.gameObject.AddComponent<SpriteRenderer>();
                    sr.sprite = prefab.GetComponent<SpriteRenderer>().sprite;
                    sr.sortingLayerName = prefab.GetComponent<SpriteRenderer>().sortingLayerName;
                    sr.sortingOrder = prefab.GetComponent<SpriteRenderer>().sortingOrder - 1;
                }
            }
            else if(prefab.tag == "Border")
            {

            }
            else if(mb.GetComponent<SpriteRenderer>() != null)
            {
                Destroy(mb.GetComponent<SpriteRenderer>());
            }           
        }
    }

    //TODO
    //添加细胞壁
    void OnEndMove(GameObject prefab, Vector3 position)
    {
        bool isOnBG = false;
        prefab.GetComponent<SpriteRenderer>().sortingOrder -= 1;
        foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
        {
            if(Vector3.Distance(mb.transform.position, prefab.transform.position) < 0.5f)
            {               
                if (mb.GetComponent<SpriteRenderer>() != null)
                {
                    Destroy(mb.GetComponent<SpriteRenderer>());
                }
                if(!mb.haveBoard && prefab.tag == "Board") 
                {
                    mb.haveBoard = true;
                    mb.boardPrefab = prefab;
                    prefab.transform.position = mb.transform.position;
                    isOnBG = true;
                }
                if (mb.haveBoard && !mb.haveCell && movePrefab.tag == "Cell")
                {
                    mb.haveCell = true;
                    mb.cellPrefab = prefab;
                    prefab.transform.position = mb.transform.position;
                    isOnBG = true;
                }               
                break;
            }
        }
        if (!isOnBG)
        {
            prefab.transform.position = position;
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if(Vector3.Distance(mb.transform.position,prefab.transform.position) < 0.3f)
                {
                    if(prefab.tag == "Cell")
                    {
                        mb.haveCell = true;
                        mb.cellPrefab = prefab;
                        break;
                    }
                    else if(prefab.tag == "Board")
                    {
                        mb.haveBoard = true;
                        mb.boardPrefab = prefab;
                        break;
                    }
                }
            }
        }
    }
    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
        DeletePrefab();
        MovePrefab();
    }
}
