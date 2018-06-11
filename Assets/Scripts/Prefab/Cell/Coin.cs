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
        if (coin.landedBoard.nearBoards[(int)dir] != null && coin.landedBoard.nearBoards[(int)dir].cellType != CellType.cell && coin.landedBoard.nearBoards[(int)dir].cellType != CellType.germ)
        {
            coin.landedBoard.isUsed = false;
            coin.landedBoard.cellType = CellType.nothing;
            Vector3 position = coin.landedBoard.nearBoards[(int)dir].transform.position;
            coin.transform.position = position;
            coin.landedBoard = coin.landedBoard.nearBoards[(int)dir];
            coin.landedBoard.isUsed = true;
            if(coin.landedBoard.cellType == CellType.nothing)
            {
                coin.landedBoard.cellType = CellType.coin;
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
