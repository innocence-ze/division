
using System.Collections.Generic;
using UnityEngine;


public class Cell : MapManagerPrefab ,ICells,IPrefab
{
    private static int MaxHealth = 2;

    // Use this for initialization
    public static List<Cell> cells = new List<Cell>();

	private static GameObject cellPrefab;
	private static void Initialize(Vector3 pos)
	{
        if (cellPrefab == null)
        {
            cellPrefab = Resources.Load<GameObject>("Cells/Cell");
        }
        Instantiate(cellPrefab, pos, new Quaternion(0, 0, 0, 0)).transform.SetParent(GameObject.Find("Cells").transform);
        
	}

    private int health;
	//[SerializeField]private Text text;
	[SerializeField]private Board landedBoard;
	public int Health
	{
		get
        {
			return health;
		}
		set
        {
            health = value;
			if(health == 0)
            {
                landedBoard.isUsed = false;
                landedBoard.cellType = CellType.nothing;
                Destroy(gameObject);
            }
		}
	}

	void Start ()
    {
        if (MapManager.instance == null)
        {
            Init();
            fre++;
        }
    }

    public void Init()
    {
        Sprite cell2 = Resources.Load<Sprite>("Material/Cell2");
        Health = MaxHealth;
        GetComponent<SpriteRenderer>().sprite = cell2;
        cells.Add(this);
        SetBoard();
    }

    int fre;
    void Update()
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
	
	void DivideTo(Vector2 position)
	{

        Vector3 pos = new Vector3(position.x, position.y, transform.position.z);
		Initialize(pos);
        Sprite cell1 = Resources.Load<Sprite>("Material/Cell1");
        GetComponent<SpriteRenderer>().sprite = cell1;
        Health--;
	}
	
	bool canMove=false;
    //判断细胞是否可以移动
	public static void MoveTo(Direction dir)
	{
		foreach(Cell cell in cells)
		{
            //判断细胞的目的地是否为null或是否已被占用
            if (cell.landedBoard.nearBoards[(int)dir] == null || (cell.landedBoard.nearBoards[(int)dir].isUsed && cell.landedBoard.nearBoards[(int)dir].cellType == CellType.cell) )           
            {
                cell.canMove = false;
            }
           
            //判断细胞的目的地是否为硬币
            else if (cell.landedBoard.nearBoards[(int)dir].isUsed && cell.landedBoard.nearBoards[(int)dir].cellType == CellType.coin)
            {
                //判断硬币被推到的地方是否为null或者已被占用
                if (cell.landedBoard.nearBoards[(int)dir].nearBoards[(int)dir] == null || (cell.landedBoard.nearBoards[(int)dir].nearBoards[(int)dir].isUsed && cell.landedBoard.nearBoards[(int)dir].nearBoards[(int)dir].cellType == CellType.cell))
                {
                    cell.canMove = false;
                }
                else
                {
                    Coin.coin.MoveTo(dir);
                    cell.canMove = true;
                }

            }

            else
            {
                cell.canMove = true;
            }
        }
		foreach(Cell cell in cells)
		{
            if (cell.canMove)
            {
                cell.DivideTo(cell.landedBoard.nearBoards[(int)dir].transform.position);
            }
		}
	}

	void OnDestroy()
	{
		cells.Remove(this);
	}

    public virtual void SetBoard()
    {
        foreach (Board b in Board.boards)
        {
            Vector3 pos = transform.position;
            Vector3 bPos = b.transform.position;
            if (Vector3.Distance(pos, bPos) < 0.5f)
            {
                landedBoard = b;
                landedBoard.isUsed = true;
                landedBoard.cellType = CellType.cell;
                break;
            }
        }
    }
}
