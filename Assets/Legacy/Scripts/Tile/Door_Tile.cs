using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Tile : Tile
{
    public Door_Tile connectedTile;

    public bool isFirst = false;
    protected override void Start()
    {
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<Door_Tile>();
        tileClass = 6;
        tileShape = 0;
    }

    protected override void Update()
    {
        if (onPlayer && !isFirst)
        {
            PlayerController.inst.transform.position = connectedTile.transform.position;
            onPlayer = false;
            connectedTile.transform.parent.gameObject.SetActive(true);
            connectedTile.isFirst = true;
            PlayerController.inst.transform.parent = connectedTile.transform.parent;
            PlayerController.inst.mapBlock = connectedTile.transform.parent.GetComponent<MapBlockManager>();
            transform.parent.gameObject.SetActive(false);
        }
        base.Update();  
    }

}
