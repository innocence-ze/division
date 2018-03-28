using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public List<Direction> stepList = new List<Direction>();  

    // Use this for initialization
    void Awake () {
        instance = this;
	}
	
    public void Victory()
    {
        print("victory");
    }

    public void Defeat()
    {
        print("defeat");
    }

	// Update is called once per frame
	void Update () {
		
	}
}
