using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerBorder : MonoBehaviour {

    public List<MapManagerBackGround> mmg = new List<MapManagerBackGround>();

    public static List<MapManagerBorder> mapManagerBorders = new List<MapManagerBorder>();

    public bool haveBorder = false;

    public GameObject borderPrefab;

    void Start () {
		foreach(var m in MapManagerBackGround.mapManagerBackGrounds)
        {
            if(Vector3.Distance(m.transform.position, transform.position) < 0.55f)
            {
                mmg.Add(m);
            }
        }
        if(mmg.Count < 2)
        {
            Destroy(gameObject);
        }
        else
        {
            mapManagerBorders.Add(this);
        }

    }

    public void FindPrefab(Transform prefab)
    {
        foreach(var m in mmg)
        {
            if (!m.haveBoard)
                return;
        }
        if(!haveBorder)
        {
            prefab.rotation = transform.rotation;
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

    public bool FixedPrefabPosition(Transform prefab)
    {
        foreach (var m in mmg)
        {
            if (!m.haveBoard)
            {
                print(m.name);
                return false;
            }
        }
        if (!haveBorder)
        {
            haveBorder = true;
            borderPrefab = prefab.gameObject;
            prefab.position = transform.position;
            prefab.rotation = transform.rotation;
            Destroy(GetComponent<SpriteRenderer>());
            SetEularAngleZ(borderPrefab);
            return true;
        }
        return false;
    }

    public void SetEularAngleZ(GameObject border)
    {
        if (transform.rotation.eulerAngles.z > 150f)
        {
            switch (border.name)
            {
                case "UpAndDown":
                    border.GetComponent<Border>().borderType = BorderDirection.UprightAndDownleft;
                    break;
                case "Up":
                    border.GetComponent<Border>().borderType = BorderDirection.Upright;
                    break;
                case "Down":
                    border.GetComponent<Border>().borderType = BorderDirection.Downleft;
                    break;
            }
        }
        else if (transform.rotation.eulerAngles.z < 30f)
        {
            switch (border.name)
            {
                case "UpAndDown":
                    border.GetComponent<Border>().borderType = BorderDirection.UpAndDown;
                    break;
                case "Up":
                    border.GetComponent<Border>().borderType = BorderDirection.Up;
                    break;
                case "Down":
                    border.GetComponent<Border>().borderType = BorderDirection.Down;
                    break;
            }
        }
        else
        {
            switch (border.name)
            {
                case "UpAndDown":
                    border.GetComponent<Border>().borderType = BorderDirection.UpleftAndDownright;
                    break;
                case "Up":
                    border.GetComponent<Border>().borderType = BorderDirection.Upleft;
                    break;
                case "Down":
                    border.GetComponent<Border>().borderType = BorderDirection.Downright;
                    break;
            }
        }
    }
}
