using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Receive_Tile : Tile
{
    protected override void Start()
    {
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<Power_Receive_Tile>();
        mapBlock.receive = GetComponent<Power_Receive_Tile>();
        tileClass = 3;
        int pathx = LevelManager.inst.PathCellarize(transform.position.x);
        int pathy = LevelManager.inst.PathCellarize(transform.position.y);
        for (int i = 0; i < 4; i++)
        {
            int temx = 0, temy = 0;
            if (i < 2)
                temx += (int)(3 * Mathf.Pow(-1, i));
            else temy += (int)(3 * Mathf.Pow(-1, i));

            if (pathx + temx >= 0 && pathx + temx < 5 * MAX_BOUND && pathy + temy >= 0 && pathy + temy < 5 * MAX_BOUND)
            {
                mapBlock.powerReceiver[i, 0] = pathx + temx;
                mapBlock.powerReceiver[i, 1] = pathy + temy;
            }
        }

    }

    protected override void Update()
    {
        base.Update();
    }
}
