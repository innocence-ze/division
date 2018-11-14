using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

//用于后台储存读取地图数据
public class MapManager : MonoBehaviour {

    [HideInInspector]static public MapManager instance;

    [HideInInspector]public List<GameObject> gamePrefabs = new List<GameObject>();

    [SerializeField]
    [Header("截屏用的相机")]
    private Camera shotCamera;

    private void Awake()
    {
        instance = this;
        shotCamera = GameObject.Find("ShotCamera").GetComponent<Camera>();
        //创建后台文件夹
        DirectoryInfo xmlFolder = new DirectoryInfo(Application.dataPath + "/Data/");
        xmlFolder.Create();
        GameObject[] go = GameObject.FindGameObjectsWithTag("BackGround");
        foreach(var g in go)
        {
            g.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(Board.boards.Count);
        }
    }

    // 存储地图信息
    public void SaveXML(string fileName)
    {
        //判断文件及文件夹是否存在，并创建文件
        string xmlPath = Application.dataPath + "/Data/" + fileName + ".xml";

        XmlDocument xmlDoc = new XmlDocument();

        XmlElement xmlData = xmlDoc.CreateElement("data");
        xmlDoc.AppendChild(xmlData);

        //地图数据  
        XmlElement xmlItems = xmlDoc.CreateElement("items");
        xmlData.AppendChild(xmlItems);

        GameObject[] gameObjectItems = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject gameObjectItem in gameObjectItems)
        {
            //储存一个gameobject 
            if (gameObjectItem.tag == "Board" || gameObjectItem.tag == "Border" || gameObjectItem.tag == "Cell" )
            {               
                XmlElement xmlItem = xmlDoc.CreateElement("item");
                xmlItems.AppendChild(xmlItem);

                XmlElement name = xmlDoc.CreateElement("name");
                name.InnerText = gameObjectItem.name;
                xmlItem.AppendChild(name);

                XmlElement tag = xmlDoc.CreateElement("tag");
                tag.InnerText = gameObjectItem.tag;
                xmlItem.AppendChild(tag);

                XmlElement positionX = xmlDoc.CreateElement("positionX");
                positionX.InnerText = gameObjectItem.transform.position.x.ToString();
                xmlItem.AppendChild(positionX);

                XmlElement positionY = xmlDoc.CreateElement("positionY");
                positionY.InnerText = gameObjectItem.transform.position.y.ToString();
                xmlItem.AppendChild(positionY);

                if(gameObjectItem.tag == "Board")
                {
                    XmlElement sprite = xmlDoc.CreateElement("sprite");
                    sprite.InnerText = gameObjectItem.GetComponent<SpriteRenderer>().sprite.name;
                    xmlItem.AppendChild(sprite);
                }    
                if(gameObjectItem.tag == "Border")
                {
                    XmlElement type = xmlDoc.CreateElement("borderType");
                    type.InnerText = gameObjectItem.GetComponent<Border>().borderType.ToString();
                    xmlItem.AppendChild(type);
                    XmlElement rotationZ = xmlDoc.CreateElement("rotationZ");
                    rotationZ.InnerText = gameObjectItem.transform.rotation.eulerAngles.z.ToString();
                    xmlItem.AppendChild(rotationZ);
                }
            }
            if(gameObjectItem.name == "BackGround")
            {
                XmlElement xmlItem = xmlDoc.CreateElement("item");
                xmlItems.AppendChild(xmlItem);

                XmlElement name = xmlDoc.CreateElement("name");
                name.InnerText = gameObjectItem.name;
                xmlItem.AppendChild(name);

                XmlElement sprite = xmlDoc.CreateElement("sprite");
                sprite.InnerText = gameObjectItem.GetComponent<Image>().sprite.name;
                xmlItem.AppendChild(sprite);
            }
        }
        xmlDoc.Save(xmlPath);
    }

    //读取本地xml文件(非试玩用)
    //TODOborder的旋转
    public void ReadXML(string fileName)
    {

        string xmlPath = Application.dataPath + "/Data/" +fileName + ".xml";
        if (File.Exists(xmlPath))
        {
            XmlDocument xmlName = new XmlDocument();
            xmlName.Load(xmlPath);
            XmlNodeList items = xmlName.SelectNodes("//item");
            foreach (XmlNode item in items)
            {
                
                XmlNode itemName = item.SelectSingleNode("name");

                GameObject readItem;
                if (itemName.InnerText != "BackGround")
                {
                    XmlNode itemTag = item.SelectSingleNode("tag");
                    XmlNode itemPositionX = item.SelectSingleNode("positionX");
                    XmlNode itemPositionY = item.SelectSingleNode("positionY");
                    Vector3 psition = new Vector3(float.Parse(itemPositionX.InnerText), float.Parse(itemPositionY.InnerText), 0);

                    string tag = itemTag.InnerText + "s";
                    string name = itemName.InnerText;
                    GameObject prefab = Resources.Load<GameObject>(tag + "/" + name);
                    readItem = Instantiate(prefab, psition, Quaternion.identity) as GameObject;
                    readItem.name = itemName.InnerText;
                    readItem.transform.SetParent(GameObject.Find(tag).transform);
                    gamePrefabs.Add(readItem);
                    if(itemTag.InnerText == "Board")
                    {
                        XmlNode itemSprite = item.SelectSingleNode("sprite");
                        readItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapManager/Board/" + itemSprite.InnerText);
                        if (name.Equals("EndBoard"))
                        {
                            haveEndboard = true;
                        }
                    }
                    else if(itemTag.InnerText == "Border")
                    {
                        XmlNode itemType = item.SelectSingleNode("borderType");
                        XmlNode itemRotZ = item.SelectSingleNode("rotationZ");
                        readItem.transform.rotation = Quaternion.Euler(0, 0, float.Parse(itemRotZ.InnerText));
                        readItem.GetComponent<Border>().borderType = (BorderDirection)Enum.Parse(typeof(BorderDirection), itemType.InnerText);
                    }
                    else
                    {
                        if(name.Contains("Cell"))
                        {
                            haveCell = true;
                            cellCount++;
                        }
                        else if (name.Equals("Coin"))
                        {
                            haveCoin = true;
                        }
                    }
                    ReadMap(readItem);
                } 
                if(itemName.InnerText == "BackGround")
                {
                    XmlNode itemSprite = item.SelectSingleNode("sprite");
                    GameObject.Find("BackGround").GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/Background/" + itemSprite.InnerText);
                    GameObject.Find("BackGroundShot").GetComponent<Image>().sprite = Resources.Load<Sprite>("MapManager/Background/" + itemSprite.InnerText);
                }
            }
        }
    }

    //读取本地xml文件(试玩用)
    public void LoadXML(string fileName)
    {
        Init("Cell");
        Init("Board");
        Init("Border");
        ReadXML(fileName);
    }

    //初始化三个空物体
    public void Init(string type)
    {
        GameObject s = GameObject.Find(type + "s");
        for(int i = 0; i < s.transform.childCount; i++)
        {
            Destroy(s.transform.GetChild(i).gameObject);
        }
        if(type == "Board")
        {
            Board.boards.Clear();
        }
        s.transform.position = new Vector3(0, 0, 0);
    }

    //读取本地文件后，设置锚点信息
    private void ReadMap(GameObject prefab)
    {
        Vector3 prefabPosition = prefab.transform.position;
        if (prefab.tag != "Border")
        {
            foreach (MapManagerBackGround mb in MapManagerBackGround.mapManagerBackGrounds)
            {
                if (Vector3.Distance(prefabPosition, mb.transform.position) <= 0.4f)
                {
                    if (prefab.tag == "Cell")
                    {
                        mb.cellPrefab = prefab;
                        mb.haveCell = true;
                        continue;
                    }
                    else if (prefab.tag == "Board")
                    {
                        mb.boardPrefab = prefab;
                        mb.haveBoard = true;
                        Board.boards.Add(mb.boardPrefab.GetComponent<Board>());
                        continue;
                    }
                }
            }
        }
        else
        {
            foreach(MapManagerBorder mb in MapManagerBorder.mapManagerBorders)
            {
                if(Vector3.Distance(prefabPosition, mb.transform.position) <= 0.2f)
                {
                    mb.haveBorder = true;
                    mb.borderPrefab = prefab;
                }
            }
        }
    }

    [SerializeField]
    private bool haveCell = false;
    [SerializeField]
    private bool haveCoin = false;
    [SerializeField]
    private bool haveEndboard = false;
    [SerializeField]
    private int cellCount = 0;

    public int CellCount
    {
        get
        {
            return cellCount;
        }
        set
        {
            cellCount = value;
            if (CellCount > 0)
            {
                HaveCell = true;
            }
            else
            {
                HaveCell = false;
            }
        }
    }
    public bool HaveCell
    {
        get
        {
            return haveCell;
        }
        set
        {
            haveCell = value;
        }
    }
    public bool HaveCoin
    {
        get
        {
            return haveCoin;
        }
        set
        {
            haveCoin = value;
        }
    }
    public bool HaveEndboard
    {
        get
        {
            return haveEndboard;
        }
        set
        {
            haveEndboard = value;
        }
    }
    
    /// <summary>
    /// 判断地图摆放是否正确
    /// </summary>
    /// <returns>是否能玩</returns>
    public bool IsRightMap()
    {
        bool isRightMap = false;       
        foreach(GameObject gamePrefab in gamePrefabs)
        {
            if(haveEndboard && haveCoin && haveCell)
            {
                isRightMap = true;
                return (isRightMap);
            }
            if(!haveCell && (gamePrefab.name == "Cell1" || gamePrefab.name == "Cell2"))
            {
                haveCell = true;
                continue;
            }
            if(!haveCoin && gamePrefab.name == "Coin")
            {
                haveCoin = true;
                continue;
            }
            if(!haveEndboard && gamePrefab.name == "EndBoard")
            {
                haveEndboard = true;
                continue;
            }
        }
        return isRightMap;
    }

    /// <summary>
    /// 将RenderTexture保存成一张png图片  
    /// </summary>
    /// <param name="fileName"></param>
    public void SavePNG(string fileName)
    {
        RenderTexture rt = new RenderTexture(Camera.main.pixelWidth,Camera.main.pixelHeight,0);
        shotCamera.targetTexture = rt;
        shotCamera.Render();
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        FileStream file = File.Open(Application.dataPath + "/Data/" + fileName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
    }

    //读取本地PNG图片
    public Sprite ReadPNG(string fileName)
    {
        FileStream file = new FileStream(Application.dataPath + "/Data/" + fileName + ".png", FileMode.Open, FileAccess.Read);
        file.Seek(0, SeekOrigin.Begin);
        byte[] bytes = new byte[file.Length];
        file.Read(bytes, 0, (int)file.Length);
        file.Close();
        file.Dispose();
        file = null;

        Texture2D texture = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
}
