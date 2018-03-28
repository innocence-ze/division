using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BorderDirection//可以通过的方向
{

    Upright,//单向通过
    Up,
    Upleft,
    Downleft,
    Down,
    Downright,

    UprightAndDownleft,//双向都不通 
    UpAndDown,
    UpleftAndDownright,

}


public class Border : MonoBehaviour {

    [NonSerialized]public GameObject[] nearBoard = new GameObject[2];

    public BorderDirection borderType;

    //寻找边界边上的两个细胞，获取后让上面的储存在board[0],下面的储存在board[1]
    void FindBoard()
    {
        int i = 0;
        foreach(Board b in Board.boards)
        {
            if (Vector3.Distance(b.transform.position, this.transform.position) <= 0.7f) 
            {
                nearBoard[i] = b.gameObject;
                i++;
                if(i == 2)
                {
                    break;
                }
            }
        }
        if(nearBoard[0].transform.position.y < nearBoard[1].transform.position.y)
        {
            var a = nearBoard[0];
            nearBoard[0] = nearBoard[1];
            nearBoard[1] = a;
        }
    }

    void InitBorder(BorderDirection bd)
    {
        int dir;
        FindBoard();
        if ((int)bd < 3)
        {
            dir = (int)bd + 3;
            nearBoard[0].GetComponent<Board>().ChangeBoard((Direction)dir);
        }
        else if((int)bd < 6)
        {
            dir = (int)bd - 3;
            nearBoard[1].GetComponent<Board>().ChangeBoard((Direction)dir);
        }
        else
        {
            dir = (int)bd - 3;
            nearBoard[0].GetComponent<Board>().ChangeBoard((Direction)dir);
            dir = dir - 3;
            nearBoard[1].GetComponent<Board>().ChangeBoard((Direction)dir);
        }
    }

	// Use this for initialization
	void Start () {
        InitBorder(borderType);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
