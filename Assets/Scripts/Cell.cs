using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Cell : MonoBehaviour {
	private static int MaxHealth=2;

	// Use this for initialization
	public static List<Cell> cells=new List<Cell>();

//	private static float moveTime=0.5f;


	private static GameObject cellPrefab;
	private static void Initialize(Vector3 pos)
	{
        if (cellPrefab==null)
        {
            cellPrefab =Resources.Load<GameObject>("Cell");
        }
        Instantiate(cellPrefab,pos,new Quaternion(0,0,0,0));
	}
	
	private int health;
	[SerializeField]private Text text;


	[SerializeField]private Board landedBoard;
	public int Health
	{
		get{
			return health;
		}
		set{
			health=value;
			
			text.text=value.ToString();
			if(health==0){
				landedBoard.isUsed=false;
                landedBoard.cellType = CellType.nothing;
				Destroy(gameObject);
				
				}
		}
	}

	void Start () {
		Health=MaxHealth;
		cells.Add(this);
		SetBoard();
	}
	
	void SetBoard()
	{
		foreach(Board b in Board.boards)
		{
			Vector3 pos=transform.position;
			Vector3 bPos=b.transform.position;
			if(Vector3.Distance(pos, bPos) < 0.5f)
			{
				landedBoard=b;
				landedBoard.isUsed=true;
                landedBoard.cellType = CellType.cell;
				break;
			}
			
		}
	}
	

	void DivideTo(Vector2 position)
	{

		Vector3 pos=new Vector3(position.x,position.y,transform.position.z);
		Initialize(pos);
		Health--;



		//transform.DOMove(pos,moveTime);
	}
	

	bool canMove=false;
	public static void MoveTo(Direction dir)
	{
		foreach(Cell cell in cells)
		{
            //判断细胞的目的地是否为null或是否已被占用
            if (cell.landedBoard.nearBoard[(int)dir] == null || (cell.landedBoard.nearBoard[(int)dir].isUsed && cell.landedBoard.nearBoard[(int)dir].cellType == CellType.cell) )           
            {
                cell.canMove = false;
            }
           
            //判断细胞的目的地是否为硬币
            else if (cell.landedBoard.nearBoard[(int)dir].isUsed && cell.landedBoard.nearBoard[(int)dir].cellType == CellType.coin)
            {
                //判断硬币被推到的地方是否为null或者已被占用
                if (cell.landedBoard.nearBoard[(int)dir].nearBoard[(int)dir] == null || (cell.landedBoard.nearBoard[(int)dir].nearBoard[(int)dir].isUsed && cell.landedBoard.nearBoard[(int)dir].nearBoard[(int)dir].cellType == CellType.cell))
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
                cell.DivideTo(cell.landedBoard.nearBoard[(int)dir].transform.position);
            }
		}



	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		cells.Remove(this);
	}
}
