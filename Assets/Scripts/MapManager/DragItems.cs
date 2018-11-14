using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//用于从UI上拖取物体到背景上
public class DragItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Dictionary<int, GameObject> draggingPrefabs = new Dictionary<int, GameObject>();

    //获取Resources文件夹下的子文件夹
    [SerializeField]
    [Header("Resources子文件夹名")]
    private string itemType;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        if( (gameObject.name == "Coin" && MapManager.instance.HaveCoin) || (gameObject.name == "EndBoard" && MapManager.instance.HaveEndboard) )
        {
            Debug.Log("Have " + gameObject.name);
            return;
        }
        if(Input.GetMouseButton(0))
        {
            //加载图片
            draggingPrefabs[eventData.pointerId] = Resources.Load<GameObject>(itemType + "/" + gameObject.name);
            if (draggingPrefabs[eventData.pointerId] != null)
            {
                draggingPrefabs[eventData.pointerId] = Instantiate(draggingPrefabs[eventData.pointerId]);
                draggingPrefabs[eventData.pointerId].GetComponent<SpriteRenderer>().sprite = image.sprite;
                draggingPrefabs[eventData.pointerId].name = gameObject.name;
                draggingPrefabs[eventData.pointerId].GetComponent<SpriteRenderer>().sortingOrder += 1;
                draggingPrefabs[eventData.pointerId].transform.SetParent(GameObject.Find(itemType).transform);
                draggingPrefabs[eventData.pointerId].transform.SetAsLastSibling();
                SetDraggingPosition(eventData);
            }
        }
    }

    //拖拽时 物体随鼠标移动
    private void SetDraggingPosition(PointerEventData eventData)
    {
       
        //鼠标拖动的物体
        var prefabTransform = draggingPrefabs[eventData.pointerId].GetComponent<Transform>();
        prefabTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        prefabTransform.position -= new Vector3(0, 0, Camera.main.transform.position.z);
        if(prefabTransform.GetComponent<Border>() == null)
        {
            foreach (MapManagerBackGround mmb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (Vector3.Distance(mmb.transform.position, prefabTransform.position) <= 0.5f)
                {
                    mmb.FindPrefab(prefabTransform);
                }
                else
                {
                    Destroy(mmb.GetComponent<SpriteRenderer>());
                }
            }
        }
        else
        {
            foreach (MapManagerBorder mmb in MapManagerBorder.mapManagerBorders)
            {
                if (Vector3.Distance(mmb.transform.position, prefabTransform.position) <= 0.2f)
                {
                    mmb.FindPrefab(prefabTransform);
                }
                else
                {
                    Destroy(mmb.GetComponent<SpriteRenderer>());
                }
            }
        }      
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && draggingPrefabs[eventData.pointerId] != null)
        {
            SetDraggingPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Transform prefab = draggingPrefabs[eventData.pointerId].transform;
        if (Input.GetMouseButtonUp(0) && prefab != null)
        {
            prefab.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            bool isOnBG = false;
            if (prefab.GetComponent<Border>() == null)
            {
                foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
                {
                    if (Vector3.Distance(prefab.position, mb.transform.position) <= 0.5f)
                    {
                        isOnBG = mb.FixedPrefabPosition(prefab);
                        if (isOnBG)
                        {
                            if (itemType == "Boards")
                            {
                                Board.boards.Add(prefab.GetComponent<Board>());
                            }
                            if (gameObject.name == "EndBoard")
                            {
                                MapManager.instance.HaveEndboard = true;
                            }
                            else if (gameObject.name == "Coin")
                            {
                                MapManager.instance.HaveCoin = true;
                            }
                            else if (gameObject.name == "Cell")
                            {
                                MapManager.instance.CellCount += 1;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
                {
                    if (Vector3.Distance(prefab.position, mb.transform.position) <= 0.2f)
                    {
                        isOnBG = mb.FixedPrefabPosition(prefab);
                    }
                }
            }
            //判断是否在锚点上 不在锚点时将物体移除队列
            if (!isOnBG)
            {
                if (prefab.GetComponent<Board>() != null)
                {
                    Board.boards.Remove(prefab.GetComponent<Board>());
                }
                if (prefab.GetComponent<Cell>() != null)
                {
                    Cell.cells.Remove(prefab.GetComponent<Cell>());
                }
                Destroy(draggingPrefabs[eventData.pointerId]);
            }
            draggingPrefabs[eventData.pointerId] = null;
        }     
    }
}
