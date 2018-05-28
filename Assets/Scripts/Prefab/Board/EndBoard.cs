using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBoard : Board {

    new void Awake()
    {
        base.Awake();
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
            if (Vector3.Distance(pos, cPos) < 0.5f)
            {
                GameManager.instance.Victory();
            }
        }
    }

    new void Update()
    {
        base.Update();
        if (MapManager.instance == null)
        {           
            Victory();
        }
    }
}
