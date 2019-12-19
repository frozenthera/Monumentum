using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_tile : Tile
{ 
    [SerializeField]
    public bool isClockWise;

    protected override void Start()
    {
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<Wall_tile>();
        tileClass = 3;
 
    }

    protected override void Update()
    {

    }
}
