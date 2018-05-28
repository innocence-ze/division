using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : PrefabInMapManager ,ICells ,IPrefab {

    private Board landedBoard;

    public WormHole partnerWormHole;

    private bool isUsed = false;

    public void SetBoard()
    {
        foreach (Board b in Board.boards)
        {
            Vector3 pos = transform.position;
            Vector3 bPos = b.transform.position;
            if (Vector3.Distance(pos,bPos) < 0.5f)
            {
                landedBoard = b;
                transform.position = bPos;
                break;
            }
        }
    }

    void Trasnsmit()
    {
        if(partnerWormHole != null && !isUsed)
        {
            foreach (Cell c in Cell.cells)
            {
                if (Vector3.Distance(c.transform.position, this.transform.position) < 0.1f)
                {                    
                    c.transform.position = partnerWormHole.transform.position;
                    isUsed = true;
                    partnerWormHole.isUsed = true;
                    this.landedBoard.cellType = CellType.nothing;
                    this.partnerWormHole.landedBoard.cellType = CellType.cell;
                    break;
                }
            }

            if (Vector3.Distance(Coin.coin.transform.position, this.transform.position) < 0.1f)
            {
                Coin.coin.transform.position = partnerWormHole.transform.position;
                isUsed = true;
                partnerWormHole.isUsed = true;
                this.landedBoard.cellType = CellType.nothing;
                this.partnerWormHole.landedBoard.cellType = CellType.coin;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (MapManager.instance == null)
        {
            Init();
            fre++;
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

            Trasnsmit();
            if (isUsed || partnerWormHole == null)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            fre = 0;
        }
    }

    public void Init()
    {
        SetBoard();
        landedBoard.isUsed = true;
        landedBoard.cellType = CellType.wormHole;
    }
}
