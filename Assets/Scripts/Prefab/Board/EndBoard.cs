using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                if(SceneManager.GetActiveScene().name != "MapManager")
                {
                    GameManager.instance.Victory();
                }
                else
                {
                    MapManagerUI.instance.Victory();
                }
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
