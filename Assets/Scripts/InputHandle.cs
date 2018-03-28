using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Direction
{
	

	Upright,
	Up,
	Upleft,
	Downleft,
	Down,
	Downright,


}



public class InputHandle : MonoBehaviour {

	// Use this for initialization

	private bool forbidden=false;
		
	void Start () {
		
	}
	Vector3 downPos;
	// Update is called once per frame
	void Update () { 

		
		if(forbidden)return;
        #if UNITY_EDITOR
		if (EventSystem.current.IsPointerOverGameObject())
			return;
        if(Input.GetMouseButtonDown(0))
        {
           downPos= Input.mousePosition;
        }
        if(Input.GetMouseButton(0))
        {
         
           
        }
		if(Input.GetMouseButtonUp(0))
		{
			Vector3 vector=Input.mousePosition-downPos;

			float angle=0;
			if(vector.y>0)
				angle =Vector2.Angle(Vector3.right,vector);
			else 
				angle=360-Vector2.Angle(Vector3.right,vector);
			Direction direction;
				 direction=(Direction)(angle/60);
            Cell.MoveTo(direction);

			//print(direction);
		}
         #endif


	}

	public void DisableInput()
	{
		forbidden=true;
	}
	public void EnableInput()
	{
		forbidden=false;
	}
}
