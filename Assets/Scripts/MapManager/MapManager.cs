using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

//用于后台储存读取地图数据
public class MapManager : MonoBehaviour {

    [HideInInspector]static public MapManager instance;

    private List<GameObject> gamePrefabs = new List<GameObject>();

    private void Start()
    {
        MapManagerUI = GameObject.FindGameObjectsWithTag("MapManager");
        instance = this;
        //创建后台文件夹
        DirectoryInfo xmlFolder = new DirectoryInfo(Application.dataPath + "/Data/");
        xmlFolder.Create();
    }

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
            if (gameObjectItem.tag == "Board" || gameObjectItem.tag == "Border" || gameObjectItem.tag == "Cell")
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
            }
        }
        xmlDoc.Save(xmlPath);
    }

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

    //初始化三个空物体
    public void Init(string type)
    {
        GameObject s = GameObject.Find(type + "s");
        for(int i = 0; i < s.transform.childCount; i++)
        {
            Destroy(s.transform.GetChild(i));

        }
        s.transform.position = new Vector3(0, 0, 0);
    }

    private GameObject[] MapManagerUI;

    [SerializeField]
    [Header("截屏用的相机")]
    private Camera shotCamera;
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
        return isRightMap;
    }

    //将RenderTexture保存成一张png图片  
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
