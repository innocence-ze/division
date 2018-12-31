using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private static InputHandle s_Instance = null;
    public static InputHandle Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(InputHandle)) as InputHandle;
            }
            if (s_Instance == null)
                Debug.Log("Can't find InputHandle");
            return s_Instance;
        }
    }

    private bool forbidden=false;
		
	Vector2 downPos;
    float angle = 0;
    Vector2 deltaPos;

    // Update is called once per frame
    void Update () {
        if (forbidden) return;


        Direction direction;
		if (EventSystem.current.IsPointerOverGameObject())
			return;
#if (UNITY_EDITOR || UNITY_STANDALONE_WIN)
        if (Input.GetMouseButtonDown(0))
        {
            downPos = Input.mousePosition;
        }
		if(Input.GetMouseButtonUp(0))
		{
            deltaPos = Input.mousePosition;
            deltaPos -= downPos;
            if (deltaPos.y > 0)
                angle = Vector2.Angle(Vector3.right, deltaPos);
            else
                angle = 360 - Vector2.Angle(Vector3.right, deltaPos);

            direction = (Direction)(angle / 60);
            if((int)direction < 6)
                Cell.MoveTo(direction);
            //text.text = downPos + "  " + deltaPos + "\n" + angle + "  " + direction;

        }
#elif (UNITY_ANDROID || UNITY_IPHONE)
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                downPos = Input.GetTouch(0).position;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                deltaPos = Input.GetTouch(0).position - downPos;
                if (deltaPos.y > 0)
                    angle = Vector2.Angle(Vector3.right, deltaPos);
                else
                    angle = 360 - Vector2.Angle(Vector3.right, deltaPos);

                direction = (Direction)(angle / 60);
                if((int)direction < 6)
                    Cell.MoveTo(direction);
            }
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
