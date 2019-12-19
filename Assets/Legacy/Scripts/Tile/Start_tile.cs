using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_tile : Tile
{
    protected override void Start()
    {
        base.Start();
        tileClass = 5;
        for (int i = 0; i < LevelManager.inst.mapList.Count; i++)
            LevelManager.inst.mapList[i].transform.gameObject.SetActive(false);

        mapBlock.gameObject.SetActive(true);
        mapBlock.tileState[posx, posy] = GetComponent<Start_tile>();
        PlayerController.inst.transform.position = transform.position;
        PlayerController.inst.mapBlock = mapBlock;
        PlayerController.inst.transform.parent = mapBlock.transform;
    }

    protected override void Update()
    {
        base.Update();
    }
}
