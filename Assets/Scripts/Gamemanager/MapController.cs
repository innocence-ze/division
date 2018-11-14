using UnityEngine;
using System.Xml;
using System.IO;
using System;

public class MapController : MonoBehaviour {

    [SerializeField]
    private string roundName;

    [SerializeField]
    private string chapter;

    public string RoundName { get { return roundName; } set { roundName = value; } }

    // Use this for initialization
    private void Start()
    {
        chapter = PlayerPrefs.GetString("CurrentChapter");
        roundName = PlayerPrefs.GetString("CurrentRound");
        TextAsset text = Resources.Load<TextAsset>("Map/" + chapter + "/" + roundName);
        using (MemoryStream stream = new MemoryStream(text.bytes))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNodeList items = doc.SelectNodes("//item");
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
                    if (itemTag.InnerText == "Board")
                    {
                        XmlNode itemSprite = item.SelectSingleNode("sprite");
                        readItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("MapManager/Board/" + itemSprite.InnerText);
                        //Board.boards.Add(readItem.GetComponent<Board>());
                    }
                    else if (itemTag.InnerText == "Border")
                    {
                        XmlNode itemType = item.SelectSingleNode("borderType");
                        XmlNode itemRotZ = item.SelectSingleNode("rotationZ");
                        readItem.transform.rotation = Quaternion.Euler(0, 0, float.Parse(itemRotZ.InnerText));
                        readItem.GetComponent<Border>().borderType = (BorderDirection)Enum.Parse(typeof(BorderDirection), itemType.InnerText);
                    }
                }
                if (itemName.InnerText == "BackGround")
                {
                    XmlNode itemSprite = item.SelectSingleNode("sprite");
                    GameObject.FindGameObjectWithTag("BackGround").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
