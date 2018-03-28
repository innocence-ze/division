using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBoard : Board {

    private void Awake()
    {
        Board.boards.Add(this);
    }

    new void Start()
    {
        base.Start();
    }

    void Victory()
    {
        if(Coin.coin!=null && Cell.cells.Count != 0)
        {
            Vector3 pos = this.transform.position;
            Vector3 cPos = Coin.coin.transform.position;
            if ((pos.x - cPos.x) * (pos.x - cPos.x) +
                (pos.y - cPos.y) * (pos.y - cPos.y)
                < 0.5f * 0.5f)
            {
                GameManager.instance.Victory();
            }
        }
    }

    private void Update()
    {
        Victory();
    }
}
