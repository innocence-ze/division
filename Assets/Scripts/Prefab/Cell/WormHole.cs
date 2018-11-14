using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : MapManagerPrefab ,ICells ,IPrefab {

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
                    landedBoard.cellType = CellType.nothing;
                    partnerWormHole.landedBoard.cellType = CellType.cell;
                    break;
                }
            }

            if (Vector3.Distance(Coin.coin.transform.position, transform.position) < 0.1f)
            {
                Coin.coin.transform.position = partnerWormHole.transform.position;
                isUsed = true;
                partnerWormHole.isUsed = true;
                landedBoard.cellType = CellType.nothing;
                partnerWormHole.landedBoard.cellType = CellType.coin;
            }

            foreach (Germ germ in Germ.germs)
            {
                if (germ != null)
                {
                    if (Vector3.Distance(germ.transform.position, transform.position) < 0.1f)
                    {
                        germ.transform.position = partnerWormHole.transform.position;
                        isUsed = true;
                        partnerWormHole.isUsed = true;
                        landedBoard.cellType = CellType.nothing;
                        partnerWormHole.landedBoard.cellType = CellType.germ;
                    }
                }
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
        GameObject[] partner = GameObject.FindGameObjectsWithTag("Cell");
        foreach(var p in partner)
        {
            if (p.GetComponent<WormHole>() != null && p.GetComponent<WormHole>() != this)
            {
                partnerWormHole = p.GetComponent<WormHole>();
            }
        }
    }
}
