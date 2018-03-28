using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : MonoBehaviour {

    private Board landedBoard;

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

    void SetPartnerHole()
    {

    }

    // Use this for initialization
    void Start ()
    {
        SetBoard();

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
