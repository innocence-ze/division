using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ChapterUI : MonoBehaviour {

    private GameObject fatherUI;
    private Image[] imgs;
    private Image mask;

	// Use this for initialization
	void Start () {
        mask = GameObject.Find("Mask").GetComponent<Image>();
        mask.DOFade(0, 0.2f);
        fatherUI = GameObject.Find("BackGround");
        if (fatherUI == null)
            return;
        imgs = fatherUI.GetComponentsInChildren<Image>();
        foreach(var img in imgs)
        {
            img.color = new Color(1, 1, 1, 0);
            img.raycastTarget = false;
            img.DOFade(1, 0.7f).onComplete = delegate ()
            {
                img.raycastTarget = true;
            };
        }
	}
	
    public void OnChangeScene()
    {
        ChapterManager cm = gameObject.GetComponent<ChapterManager>();
        if(cm == null)
        {
            gameObject.AddComponent<ChapterManager>();
        }
        mask.DOFade(1, 0.5f).onComplete = delegate ()
         {
             if (EventSystem.current.currentSelectedGameObject.name == "Return")
                 cm.OnReturnMain();
             else
                 cm.OnChooseRound();
         };
    }

	// Update is called once per frame
	void Update () {
		
	}
}
