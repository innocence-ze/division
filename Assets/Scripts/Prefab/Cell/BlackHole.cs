﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : PrefabInMapManager,ICells ,IPrefab{

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
            if (Vector3.Distance(c.transform.position, this.transform.position) < 0.1f)
            {
                c.Health = 0;
                break;
            }
        }

        if (Vector3.Distance(Coin.coin.transform.position, this.transform.position) < 0.1f)
        {
            Destroy(Coin.coin.gameObject);
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
            else
            {
                GameManager.instance.Defeat();
            }
            SetBoard();
        }
        else
        {
            fre = 0;
        }
    }


}