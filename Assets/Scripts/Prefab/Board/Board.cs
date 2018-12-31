using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    nothing,   
    blackHole,
    wormHole,
    cell,
    coin,
    germ,
}


public class Board : MapManagerPrefab, IPrefab 
{

    ///两个细胞的间距
    ///qxx,qxy是斜着的两个的间距
    ///x,y是直着的两个的间距
    public static float QXX = 0.866f;
    public static float QXY = 0.5f;
    public static float X = 0.0f;
    public static float Y = 1.0f;

    // Use this for initialization
    public static List<Board> boards = new List<Board>();

	public Board[] nearBoards = new Board[6];

    //得到6个方向上的细胞
    void InitBoards(Direction dir)
	{
		float x=0,y=0;
		
		switch (dir)
		{
			case Direction.Upright:
                x = QXX; y = QXY;
			
				break;
			case Direction.Up:
                x = X; y = Y;
			
				break;
			case Direction.Upleft:
                x = -QXX; y = QXY;
			
				break;
			case Direction.Downleft:
                x = -QXX; y = -QXY;
				
				break;
			case Direction.Down:
                x = -X; y = -Y;
			
				break;
			case Direction.Downright:
                x = QXX; y = -QXY;
				
				break;
		}
        InitBoard(ref nearBoards[(int)dir],x,y);
		
	}

	void InitBoard(ref Board board,float x,float y)
    {
		foreach(Board b in boards)
		{
			if(Mathf.Abs(b.transform.position.x - transform.position.x - x) < 0.01f &&
            Mathf.Abs(b.transform.position.y - transform.position.y - y) < 0.01f) 
			{
				board=b; break;
			}
		}
	}


    //删除当前方向的地板
    public void ChangeBoard(Direction dir)
    {
        nearBoards[(int)dir] = null;
    }

	protected void Awake()
	{
        if (MapManager.instance == null)
        {
            boards.Add(this);
        }
	}

    protected void Start ()
    {

        if(MapManager.instance == null)
        {
            Init();
            fre++;
        }
    }

    int fre = 0;
    protected void Update()
    {
        if (MapManager.instance == null)
        {
            if(fre == 0)
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

    public void Init()
    {
        for (int i = 0; i < 6; i++)
        {
            InitBoards((Direction)i);
        }
    }

    //判断是不是单独的地板
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

    public bool isUsed = false;
    public CellType cellType = CellType.nothing;
}
