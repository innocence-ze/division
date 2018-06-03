using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//用于从UI上拖取物体到背景上
public class DragItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Dictionary<int, GameObject> draggingPrefabs = new Dictionary<int, GameObject>();

    //获取Resources文件夹下的子文件夹
    public string itemType;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Image image = GetComponent<Image>();
        if(Input.GetMouseButton(0))
        {
            //加载图片
            draggingPrefabs[eventData.pointerId] = Resources.Load<GameObject>(itemType + "/" + gameObject.name);
            if (draggingPrefabs[eventData.pointerId] != null)
            {
                draggingPrefabs[eventData.pointerId] = Instantiate(draggingPrefabs[eventData.pointerId]);
                draggingPrefabs[eventData.pointerId].GetComponent<SpriteRenderer>().sprite = image.sprite;
                draggingPrefabs[eventData.pointerId].name = gameObject.name;
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
        foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
        {
            if(itemType != "Borders")
            {
                mb.FindPrefab(prefabTransform);
            }
            else
            {
                mb.FindPrefab(prefabTransform, prefabTransform.GetComponent<Border>().borderType);
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
        if(Input.GetMouseButtonUp(0))
        {
            bool isOnBG = false;
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (itemType != "Borders")
                {
                    isOnBG = mb.FixedPrefabPosition(draggingPrefabs[eventData.pointerId].transform);
                    if (isOnBG)
                    {
                        if(itemType == "Boards")
                        {
                            Board.boards.Add(draggingPrefabs[eventData.pointerId].GetComponent<Board>());
                        }
                        break;
                    }
                }
                else
                {
                    isOnBG = mb.FixedPrefabPosition(draggingPrefabs[eventData.pointerId].transform, draggingPrefabs[eventData.pointerId].GetComponent<Border>().borderType);
                    if (isOnBG)
                        break;
                }
            }
            //判断是否在锚点上 不在锚点时
            if (!isOnBG)
            {
                if (draggingPrefabs[eventData.pointerId].GetComponent<Board>() != null)
                {
                    Board.boards.Remove(draggingPrefabs[eventData.pointerId].GetComponent<Board>());
                }
                if (draggingPrefabs[eventData.pointerId].GetComponent<Cell>() != null)
                {
                    Cell.cells.Remove(draggingPrefabs[eventData.pointerId].GetComponent<Cell>());
                }
                Destroy(draggingPrefabs[eventData.pointerId]);
            }
            draggingPrefabs[eventData.pointerId] = null;
        }     
    }
}
