using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Board landedBoard;
    public static Coin coin;

	// Use this for initialization
	void Start ()
    {
        coin = this;
        SetBoard();
	}

    void SetBoard()
    {
        foreach (Board b in Board.boards)
        {
            Vector3 pos = transform.position;
            Vector3 bPos = b.transform.position;
            if (Vector3.Distance(pos,bPos)< 0.5f)
            {
                landedBoard = b;
                landedBoard.isUsed = true;
                landedBoard.cellType = CellType.coin;
                transform.position = bPos;
                break;
            }

        }
    }

    public void MoveTo(Direction dir)
    {
        if (coin.landedBoard.nearBoard[(int)dir] != null && coin.landedBoard.nearBoard[(int)dir].cellType != CellType.cell)
        {
            coin.landedBoard.isUsed = false;
            coin.landedBoard.cellType = CellType.nothing;
            Vector3 position = coin.landedBoard.nearBoard[(int)dir].transform.position;
            coin.transform.position = position;
            coin.landedBoard = coin.landedBoard.nearBoard[(int)dir];
            coin.landedBoard.isUsed = true;
            if(coin.landedBoard.cellType == CellType.nothing)
            {
                coin.landedBoard.cellType = CellType.coin;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {

	}
}
