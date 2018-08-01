using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Germ : MapManagerPrefab, ICells, IPrefab
{
    public static List<Germ> germs = new List<Germ>();
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
        germs.Add(this);
        SetBoard();
        landedBoard.cellType = CellType.germ;
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
            if (landedBoard.cellType == CellType.nothing)
            {
                landedBoard.cellType = CellType.germ;
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
                if (SceneManager.GetActiveScene().name != "MapManager")
                    GameManager.instance.Defeat();
                else
                {
                    MapManagerUI.instance.Defeat();
                }
            }
        }
        else
        {
            fre = 0;
        }
    }

    void OnDestroy()
    {
        landedBoard.isUsed = false;
        landedBoard.cellType = CellType.nothing;
        germs.Remove(this);
    }
}
