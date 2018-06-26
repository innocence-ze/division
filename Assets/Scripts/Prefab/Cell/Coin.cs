using UnityEngine;

public class Coin : MapManagerPrefab,ICells , IPrefab
{
    [SerializeField]private Board landedBoard;
    public static Coin coin;

	// Use this for initialization
	protected void Start ()
    {
        if (MapManager.instance == null)
        {
            Init();
            fre++;
        }
    }

    public void Init()
    {
        coin = this;
        SetBoard();
        landedBoard.cellType = CellType.coin;
    }

    public void SetBoard()
    {
        foreach (Board b in Board.boards)
        {
            Vector3 pos = transform.position;
            Vector3 bPos = b.transform.position;
            if (Vector3.Distance(pos,bPos)< 0.5f)
            {
                landedBoard = b;
                landedBoard.isUsed = true;

                transform.position = bPos;
                break;
            }

        }
    }

    public void MoveTo(Direction dir)
    {
        if (landedBoard.nearBoards[(int)dir] != null && landedBoard.nearBoards[(int)dir].cellType != CellType.cell && landedBoard.nearBoards[(int)dir].cellType != CellType.germ)
        {
            landedBoard.isUsed = false;
            landedBoard.cellType = CellType.nothing;
            Vector3 position = landedBoard.nearBoards[(int)dir].transform.position;
            transform.position = position;
            landedBoard = landedBoard.nearBoards[(int)dir];
            landedBoard.isUsed = true;
            if(landedBoard.cellType == CellType.nothing)
            {
                landedBoard.cellType = CellType.coin;
            }
        }
    }

    // Update is called once per frame
    int fre = 0;
    protected void Update ()
    {
        if (MapManager.instance == null)
        {
            if (fre == 0)
            {
                Init();
                fre++;
            }

            SetBoard();
        }
        else
        {
            fre = 0;
        }
    }
}
