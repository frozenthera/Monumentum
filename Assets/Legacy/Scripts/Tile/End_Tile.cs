using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_Tile : Tile
    {
    protected override void Start()
    {
        base.Start();
        tileClass = 6;
        mapBlock.tileState[posx, posy] = GetComponent<End_Tile>();
    }

    protected override void Update()
    {
        base.Update();
        if (onPlayer)
        {
            Destroy(PlayerController.inst.transform.gameObject);
        }
    }
}
