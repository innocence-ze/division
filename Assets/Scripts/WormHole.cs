using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : MonoBehaviour {

    private Board landedBoard;

    public WormHole partnerWormHole;

    private bool isUsed = false;

    void SetBoard()
    {
        foreach (Board b in Board.boards)
        {
            Vector3 pos = transform.position;
            Vector3 bPos = b.transform.position;
            if (Vector3.Distance(pos,bPos) < 0.5f)
            {
                landedBoard = b;
                landedBoard.isUsed = true;
                landedBoard.cellType = CellType.wormHole;
                transform.position = bPos;
                break;
            }
        }
    }

    void Trasnsmit()
    {
        if(partnerWormHole != null)
        {
            foreach (Cell c in Cell.cells)
            {
                if (Vector3.Distance(c.transform.position, this.transform.position) < 0.1f && isUsed == false)
                {
                    c.transform.position = partnerWormHole.transform.position;
                    isUsed = true;
                    partnerWormHole.isUsed = true;
                    break;
                }
            }

            if (Vector3.Distance(Coin.coin.transform.position, this.transform.position) < 0.1f && isUsed == false)
            {
                Coin.coin.transform.position = partnerWormHole.transform.position;
                isUsed = true;
                partnerWormHole.isUsed = true;
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        SetBoard();
        isUsed = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isUsed || partnerWormHole == null)
        {
            Destroy(gameObject);
        }
        Trasnsmit();
	}
}
