using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

//用于后台储存读取地图数据
public class MapManager : MonoBehaviour {

    [HideInInspector]static public MapManager instance;

    [HideInInspector] public List<GameObject> gamePrefabs = new List<GameObject>();


    private void Start()
    {
        MapManagerUI = GameObject.FindGameObjectsWithTag("MapManager");
        instance = this;
    }

    private void Update()
    {        
        
    }

    public Text text;

    public void SaveXML()
    {
        //判断文件及文件夹是否存在，并创建文件夹 
        DirectoryInfo xmlFolder = new DirectoryInfo(Application.dataPath + "/Data/");
        xmlFolder.Create();
        string xmlPath = Application.dataPath + "/Data/" + text.text + ".xml";
        for (int i = 1; File.Exists(xmlPath); i++)
        {
            xmlPath = Application.dataPath + "/Data/" + text.text  + "(" +  i +")"+ ".xml";
        }

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
            if (gameObjectItem.tag == "Board" || gameObjectItem.tag == "Border" || gameObjectItem.tag == "Cell")
            {
                Debug.Log("要保存的数据：" + gameObjectItem.name + "类型" + gameObjectItem.tag + "位置：" + gameObjectItem.transform.position.x + "," + gameObjectItem.transform.position.y);

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
            }
        }
        xmlDoc.Save(xmlPath);
        Debug.Log("创建XML完毕");
    }

    private void ReadXml()
    {

        string xmlPath = Application.dataPath + "/Data/" + text.text + ".xml";
        if (File.Exists(xmlPath))
        {
            XmlDocument xmlName = new XmlDocument();
            xmlName.Load(xmlPath);
            XmlNodeList items = xmlName.SelectNodes("//item");
            foreach (XmlNode item in items)
            {
                
                XmlNode itemName = item.SelectSingleNode("name");
                XmlNode itemTag = item.SelectSingleNode("tag");
                XmlNode itemPositionX = item.SelectSingleNode("positionX");
                XmlNode itemPositionY = item.SelectSingleNode("positionY");

                Vector3 psition = new Vector3(float.Parse(itemPositionX.InnerText), float.Parse(itemPositionY.InnerText), 0);
                
                string tag = itemTag.InnerText + "s";
                string name = itemName.InnerText;
                GameObject prefab = Resources.Load<GameObject>(tag + "/" + name);
                GameObject readItem = Instantiate(prefab, psition, Quaternion.identity) as GameObject;
                readItem.name = itemName.InnerText;
                readItem.transform.SetParent(GameObject.FindWithTag(itemTag.InnerText).transform);
            }
        }
    }

    private void Init(string type)
    {
        GameObject s = new GameObject(type + "s")
        {
            tag = type
        };
        s.transform.position = new Vector3(0, 0, 0);
    }

    GameObject[] MapManagerUI;

    public void ReadMap()
    {
        Init("Border");
        Init("Board");
        Init("Cell");

        GameObject[] gameObjectItems = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject gameObjectItem in gameObjectItems)
        {
            //储存一个gameobject 
            if (gameObjectItem.tag != "Board" || gameObjectItem.tag != "Border" || gameObjectItem.tag != "Cell" || gameObjectItem.tag != "BackGround" ||  gameObjectItem.tag != "MainCamera")
            {
                {
                    gameObjectItem.SetActive(false);
                }
            }
        }

        var gM = Resources.Load("GameManager");
        GameObject gameManager = Instantiate(gM) as GameObject;
        gameManager.name = "GameManager";
        ReadXml();
    }

    public Camera shotCamera;
    public void Play()
    {
        
        if(IsRightMap())
        {            
            foreach (GameObject m in MapManagerUI)
            {
                m.SetActive(false);
            }
            instance = null;
            GameObject GameManager = Resources.Load<GameObject>("GameManager");
            Instantiate(GameManager, new Vector3(),new Quaternion());           
        }
        else
        {

        }
    }

    bool IsRightMap()
    {
        bool isRightMap = false;
        bool haveCell = false;
        bool haveCoin = false;
        bool haveEndboard = false;
        foreach(GameObject gamePrefab in gamePrefabs)
        {
            if(haveEndboard && haveCoin && haveCell)
            {
                isRightMap = true;
                return (isRightMap);
            }
            if(!haveCell && gameObject.name == "Cell")
            {
                haveCell = true;
                continue;
            }
            if(!haveCoin && gameObject.name == "Coin")
            {
                haveCoin = true;
                continue;
            }
            if(!haveEndboard && gameObject.name == "EndBoard")
            {
                haveEndboard = true;
                continue;
            }
        }
        return true;
    }

    //将RenderTexture保存成一张png图片  
    public bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;
    }
}
