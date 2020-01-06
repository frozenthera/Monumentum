using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Door_Tile : Tile
{
    public Door_Tile connectedTile;

    public bool isFirst = false;
    protected override void Start()
    {
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<Power_Door_Tile>();
        tileClass = 6;
        tileShape = 1;
    }

    protected override void Update()
    {
        base.Update();
        if (onPlayer && !isFirst && mapBlock.canEnd)
        {
            PlayerController.inst.transform.position = connectedTile.transform.position;
            onPlayer = false;
            connectedTile.transform.parent.gameObject.SetActive(true);
            connectedTile.isFirst = true;
            PlayerController.inst.transform.parent = connectedTile.transform.parent;
            PlayerController.inst.mapBlock = connectedTile.transform.parent.GetComponent<MapBlockManager>();
            transform.parent.gameObject.SetActive(false);
        }
    }


}
