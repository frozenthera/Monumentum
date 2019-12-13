using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : Tile
{
   
    protected override void Start()
    {
        onPower = transform.Find("onPower").gameObject;
        onPower.SetActive(false);
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<NormalTile>();
        tileClass = 1;

    }

    protected override void Update()
    {
        base.Update();
        if (onPower != null)
        {
            if (isPower && !isMoving)
                onPower.SetActive(true);
            else
                onPower.SetActive(false);
        }
    }
}
