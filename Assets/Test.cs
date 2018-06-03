using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print(transform.childCount);
        print(transform.GetChild(0).gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {

	}
}
