using System.Collections;
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
        var nearBoard = landedBoard.nearBoards[(int)dir];
        if (nearBoard != null && nearBoard.cellType != CellType.cell && nearBoard.cellType != CellType.germ)
        {
            landedBoard.isUsed = false;
            landedBoard.cellType = CellType.nothing;
            Vector3 position = nearBoard.transform.position;
            StartCoroutine(Move(position));
            landedBoard = nearBoard;
            landedBoard.isUsed = true;
            if(landedBoard.cellType == CellType.nothing)
            {
                landedBoard.cellType = CellType.coin;
            }
        }
    }


    IEnumerator Move(Vector3 position)
    {
        for(; Vector3.Distance(transform.position,position)> 0.1f;)
        {
            var x = Mathf.Lerp(transform.position.x, position.x, 0.7f);
            var y = Mathf.Lerp(transform.position.y, position.y, 0.7f);
            transform.position = new Vector3(x, y, transform.position.z);
            InputHandle.Instance.DisableInput();
            yield return null;
        }
        transform.position = position;
        InputHandle.Instance.EnableInput();
        yield return null;
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
                SetBoard();
            }
            if (Vector3.Distance(transform.position, landedBoard.transform.position) > 0.5f)
                InputHandle.Instance.DisableInput();
            else
                InputHandle.Instance.EnableInput();
            
        }
        else
        {
            fre = 0;
        }
    }
}
