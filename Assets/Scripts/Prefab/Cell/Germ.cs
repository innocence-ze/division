using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Germ : MapManagerPrefab, ICells, IPrefab
{
    public static Germ germ;
    [SerializeField] private Board landedBoard;

    private void Start()
    {
        if (MapManager.instance == null)
        {
            Init();
            fre++;
        }
    }

    public void Init()
    {
        germ = this;
        SetBoard();
        landedBoard.cellType = CellType.germ;
    }

    public void MoveTo(Direction dir)
    {
        if (germ.landedBoard.nearBoards[(int)dir] != null && germ.landedBoard.nearBoards[(int)dir].cellType != CellType.cell && germ.landedBoard.nearBoards[(int)dir].cellType != CellType.germ)
        {
            germ.landedBoard.isUsed = false;
            germ.landedBoard.cellType = CellType.nothing;
            Vector3 position = germ.landedBoard.nearBoards[(int)dir].transform.position;
            germ.transform.position = position;
            germ.landedBoard = germ.landedBoard.nearBoards[(int)dir];
            germ.landedBoard.isUsed = true;
            if (germ.landedBoard.cellType == CellType.nothing)
            {
                germ.landedBoard.cellType = CellType.germ;
            }
        }
    }

    public void SetBoard()
    {
        foreach (Board b in Board.boards)
        {
            Vector3 pos = transform.position;
            Vector3 bPos = b.transform.position;
            if (Vector3.Distance(pos, bPos) < 0.5f)
            {
                landedBoard = b;
                landedBoard.isUsed = true;

                transform.position = bPos;
                break;
            }

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

            SetBoard();
            if(landedBoard.GetComponent<EndBoard>() != null)
            {
                GameManager.instance.Defeat();
            }
        }
        else
        {
            fre = 0;
        }
    }
}
