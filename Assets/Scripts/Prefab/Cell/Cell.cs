using System.Collections.Generic;
using UnityEngine;


public class Cell : MapManagerPrefab ,ICells,IPrefab
{

    private Animator anim;
    private static int MaxHealth = 2;

    // Use this for initialization
    public static List<Cell> cells = new List<Cell>();

	private static GameObject cellPrefab;
	private static void Initialize(Vector3 pos)
	{
        if (cellPrefab == null)
        {
            cellPrefab = Resources.Load<GameObject>("Cells/Cell2");
        }
        GameObject g = Instantiate(cellPrefab, pos, new Quaternion(0, 0, 0, 0));
        g.transform.SetParent(GameObject.Find("Cells").transform);
        g.name = "Cell2";

    }

    [SerializeField]
    private Board landedBoard;

    public Board LandBoard { get { return landedBoard; } }

    [SerializeField]
    private int health;
	public int Health
	{
		get
        {
			return health;
		}
		set
        {
            health = value;
            anim.SetInteger("Health", health);
            if (health == 0)
            {               
                landedBoard.isUsed = false;
                landedBoard.cellType = CellType.nothing;
                InputHandle.Instance.EnableInput();
                Destroy(gameObject, 1f);
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
        anim = GetComponent<Animator>();
        //Sprite cell2 = Resources.Load<Sprite>("Material/Cell2");
        if(gameObject.name == "Cell2")
        {
            Health = MaxHealth;
        }
        if(gameObject.name == "Cell1")
        {
            Health = 1;
        }
        //GetComponent<SpriteRenderer>().sprite = cell2;
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
	
    //细胞朝固定方向移动
	public void DivideTo(Vector2 position)
	{

        Vector3 pos = new Vector3(position.x, position.y, transform.position.z);
		Initialize(pos);
        Sprite cell1 = Resources.Load<Sprite>("Material/Cell1");
        GetComponent<SpriteRenderer>().sprite = cell1;
        Health--;
	}
	
    [SerializeField]
	bool isMove=false;
    //判断细胞是否可以移动
	public static void MoveTo(Direction dir)
	{
		foreach(Cell cell in cells)
		{
            var nearBoard = cell.landedBoard.nearBoards[(int)dir];
            //判断细胞的目的地是否为null
            if (nearBoard == null)           
            {
                cell.isMove = false;
            }

            //判断细胞的目的地是否为细胞
            else if(nearBoard.isUsed && nearBoard.cellType == CellType.cell)
            {
                cell.isMove = false;
            }

            //判断细胞的目的地是否为硬币
            else if (nearBoard.isUsed && (int)nearBoard.cellType >= 4)
            {
                //判断硬币被推到的地方是否为null或者已被占用
                if (nearBoard.nearBoards[(int)dir] == null)
                {
                    cell.isMove = false;
                }
                else if(nearBoard.nearBoards[(int)dir].isUsed && (int)nearBoard.nearBoards[(int)dir].cellType >= 3)
                {
                    cell.isMove = false;
                }
                else
                {                   
                    cell.anim.SetInteger("PushType", (int)nearBoard.cellType);
                    cell.isMove = true;
                }

            }

            else
            {
                cell.isMove = true;
            }

            cell.anim.SetBool("IsMove", cell.isMove);
            cell.anim.SetInteger("MoveDir", (int)dir);
        }
        		
	}

	void OnDestroy()
	{
		cells.Remove(this);
	}

    public virtual void SetBoard()
    {
        if (Health == 0)
            return;
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
