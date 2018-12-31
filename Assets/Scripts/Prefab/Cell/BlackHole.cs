using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHole : MapManagerPrefab,ICells ,IPrefab{

    [SerializeField]private Board landedBoard;

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
                landedBoard.cellType = CellType.blackHole;
                transform.position = bPos;
                break;
            }
        }
    }

    public void Init()
    {
        SetBoard();
    }

    // Use this for initialization
    void Start()
    {
        if (MapManager.instance == null)
        {
            Init();
            fre++;
        }
    }

    void EatCell()
    {
        foreach (Cell c in Cell.cells)
        {
            if (Vector3.Distance(c.transform.position, transform.position) < 0.1f)
            {
                c.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                c.Health = 0;
                break;
            }
        }

        if (Vector3.Distance(Coin.coin.transform.position, transform.position) < 0.1f)
        {
            Destroy(Coin.coin.gameObject);
            GameManager.instance.Defeat();
        }
        
        if (Germ.germs != null)
        {
            foreach(Germ germ in Germ.germs)
            {
                if (Vector3.Distance(germ.transform.position, transform.position) < 0.1f)
                {
                    Destroy(germ.gameObject);
                }
            }           
        }      
    }

    // Update is called once per frame
    int fre = 0;
	void Update ()
    {
        if (MapManager.instance == null)
        {
            if (fre == 0)
            {
                Init();
                fre++;
            }
            
            if (Cell.cells.Count != 0 && Coin.coin != null)
            {
                EatCell();
            }           
            SetBoard();
        }
        else
        {
            fre = 0;
        }
    }


}
