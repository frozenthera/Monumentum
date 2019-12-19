using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Source_Tile : Tile
{
    protected override void Start()
    {
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<Power_Source_Tile>();
        mapBlock.source = GetComponent<Power_Source_Tile>();
        tileClass = 7;
        mapBlock.canEnd = false;
        int pathx = LevelManager.inst.PathCellarize(transform.localPosition.x);
        int pathy = LevelManager.inst.PathCellarize(transform.localPosition.y);
        for (int i = 0; i < 4; i++)
        {
            int temx = 0, temy = 0;
            if (i < 2)
                temx += (int)(2 * Mathf.Pow(-1, i));
            else temy += (int)(2 * Mathf.Pow(-1, i));

            if (pathx + temx >= 0 && pathx + temx < 5 * MAX_BOUND && pathy + temy >= 0 && pathy + temy < 5 * MAX_BOUND)
            {
                mapBlock.powerSource[i, 0] = pathx + temx;
                mapBlock.powerSource[i, 1] = pathy + temy;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
