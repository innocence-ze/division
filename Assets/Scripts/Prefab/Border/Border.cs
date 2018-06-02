
using UnityEngine;


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


public class Border : PrefabInMapManager ,IPrefab
{

    public Board[] nearBoards = new Board[2];

    public BorderDirection borderType;

    //寻找边界边上的两个细胞，获取后让上面的储存在board[0],下面的储存在board[1]
    void FindBoard()
    {
        int i = 0;
        foreach(Board b in Board.boards)
        {
            if (Vector3.Distance(b.transform.position, this.transform.position) <= 0.7f) 
            {
                nearBoards[i] = b;
                i++;
                if(i == 2)
                {
                    break;
                }
            }
        }
        if (nearBoards[0] != null && nearBoards[1] != null)
        {
            if (nearBoards[0].transform.position.y < nearBoards[1].transform.position.y)
            {
                var a = nearBoards[0];
                nearBoards[0] = nearBoards[1];
                nearBoards[1] = a;
            }
        }
    }

    void InitBorder(BorderDirection bd)
    {
        int dir;
        FindBoard();
        if (nearBoards[0] != null && nearBoards[1] != null)
        {
            if ((int)bd < 3)
            {
                dir = (int)bd + 3;
                nearBoards[0].GetComponent<Board>().ChangeBoard((Direction)dir);
            }
            else if ((int)bd < 6)
            {
                dir = (int)bd - 3;
                nearBoards[1].GetComponent<Board>().ChangeBoard((Direction)dir);
            }
            else
            {
                dir = (int)bd - 3;
                nearBoards[0].GetComponent<Board>().ChangeBoard((Direction)dir);
                dir = dir - 3;
                nearBoards[1].GetComponent<Board>().ChangeBoard((Direction)dir);
            }
        }           
    }

    public void Init()
    {
        InitBorder(borderType);
    }

    // Use this for initialization
    void Start () {
        if (MapManager.instance == null)
        {
            Init();
            fre++;
        }
    }

    int fre = 0;
    void Update()
    {
        if (MapManager.instance == null)
        {
            if (fre == 0)
            {               
                Init();
                fre++;
            }
        }
        else
        {
            fre = 0;
        }
    }

    public bool HaveNearBoard()
    {
        foreach (Board nearBoard in nearBoards)
        {
            if (nearBoard != null)
            {
                return true;
            }
        }
        return false;
    }
}
