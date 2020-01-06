using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlockManager : MonoBehaviour
{
    [SerializeField]
    float offset = 2f;

    public int MAX_BOUND = 10;
    public int curposx, curposy;
    public int[,] powerReceiver = new int[4, 2];
    public int[,] powerSource = new int[4, 2];

    public Tile curTile;
    public Tile source, receive;
    public Tile[,] tileState;
    public List<RotateTile> shouldRotate;

    public GameObject curTarget = null;

    public Vector3 UpMousePosition;
    public Vector3 MousePosition;

    public bool[,] tmp = new bool[5, 5];
    public bool[,] movableSpace;
    public bool[,] onPowerSpace;
    public bool canRotate;
    public bool canEnd;

    public void Awake()
    {
        tileState = new Tile[MAX_BOUND, MAX_BOUND];
        movableSpace = new bool[MAX_BOUND * 5, MAX_BOUND * 5];
        onPowerSpace = new bool[5 * MAX_BOUND, 5 * MAX_BOUND];
        canEnd = true;
    }

    void Start()
    {

    }

    void Update()
    {
        CheckPlayer();
        CheckCanMove();
        CheckCanRotate();
        UpdateMovableSpace();
        UpdatePowerCircuit();
    }


    private void CheckCanMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition = Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z);
            MousePosition = LevelManager.inst.Camera.ScreenToWorldPoint(MousePosition);
            RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector3.down);

            if (hit.collider != null)
            {
                curTarget = hit.collider.gameObject;
                for (int i = 0; i < MAX_BOUND; i++)
                    for (int j = 0; j < MAX_BOUND; j++)
                        if (curTarget != null && tileState[i, j] != null)
                            if (curTarget == tileState[i, j].gameObject)
                                curTile = tileState[i, j];
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            UpMousePosition = Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z);
            UpMousePosition = LevelManager.inst.Camera.ScreenToWorldPoint(UpMousePosition);
            Vector3 moveTo = UpMousePosition - MousePosition;

            if (curTile != null)
                if (curTile.tileClass != 3 && !curTile.onPlayer && !curTile.isMoving && curTile.tileClass != 6 && curTile.tileClass != 7 && curTile.tileClass != 5)
                {
                    curposx = curTile.posx;
                    curposy = curTile.posy;
                    curTile.MoveTile(NormalizeDir(moveTo));
                }
        }
    }

    public void CheckCanRotate()
    {
        if (curTarget != null)
        {
            if(curTile != null)
                if (curTile.tileClass == 3)
                {
                    canRotate = false;
                    for (int i = 0; i < 4; i++)
                    {
                        int temx = 0, temy = 0;
                        if (i < 2)
                            temx = (int)(3 * Mathf.Pow(-1, i));
                        else temy = (int)(3 * Mathf.Pow(-1, i));
                        if (LevelManager.inst.PathCellarize(curTile.transform.localPosition.x) + temx == PlayerController.inst.pathposx && LevelManager.inst.PathCellarize(curTile.transform.localPosition.y) + temy == PlayerController.inst.pathposy)
                        {
                            canRotate = true;
                            break;
                        }
                    }

                    if (canRotate && curTarget.transform.Find("ClockWise"))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            int tmpx = curTile.posx, tmpy = curTile.posy;
                            if (i < 2)
                                tmpx += (int)Mathf.Pow(-1, i);
                            else tmpy += (int)Mathf.Pow(-1, i);

                            if (tmpx > -1 && tmpy > -1 && tmpx < MAX_BOUND && tmpy < MAX_BOUND)
                                if (tileState[tmpx, tmpy] != null)
                                {
                                    if (tileState[tmpx, tmpy].tileClass == 2)
                                        curTile.surround[i] = tileState[tmpx, tmpy].GetComponent<RotateTile>();
                                }
                                else { curTile.surround[i] = null; }
                        }
                        for (int i = 0; i < 4; i++)
                            if (curTile.surround[i] != null)
                                if (curTile.surround[i].isRotate == false)
                                    RotateCheck(curTile.surround[i], false);
                    }
                    else if (canRotate && curTarget.transform.Find("CounterClockWise"))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            int tmpx = curTile.posx, tmpy = curTile.posy;
                            if (i < 2)
                                tmpx += i % 2 == 0 ? -1 : 1;
                            else tmpy += (int)Mathf.Pow(-1, i);

                            if (tmpx > -1 && tmpy > -1 && tmpx < MAX_BOUND && tmpy < MAX_BOUND)
                                if (tileState[tmpx, tmpy] != null)
                                {
                                    if (tileState[tmpx, tmpy].tileClass == 2)
                                        curTile.surround[i] = tileState[tmpx, tmpy].GetComponent<RotateTile>();
                                }
                                else { curTile.surround[i] = null; }
                        }
                        for (int i = 0; i < 4; i++)
                            if (curTile.surround[i] != null)
                                if (curTile.surround[i].isRotate == false)
                                    RotateCheck(curTile.surround[i], true);
                    }
                }


            for (int i = 0; i < shouldRotate.Count; i++)
            {
                if (!shouldRotate[i].isRotating)
                {
                    shouldRotate[i].Rotate();
                }
                shouldRotate[i].isRotate = false;
            }
            shouldRotate.Clear();
            curTarget = null;

            UpdateMovableSpace();
        }
    }

    private void RotateCheck(RotateTile obj, bool _dir)
    {
        if (!obj.isRotate)
        {
            obj.isRotate = true;
            shouldRotate.Add(obj.GetComponent<RotateTile>());
            obj.dir = _dir;

            for (int i = 0; i < 4; i++)
            {
                int tmpx = obj.posx, tmpy = obj.posy;
                 if (i < 2)
                    tmpx += (int)Mathf.Pow(-1, i);
                else tmpy += (int)Mathf.Pow(-1, i);

                if (tmpx > -1 && tmpy > -1 && tmpx < MAX_BOUND && tmpy < MAX_BOUND)
                    if (tileState[tmpx, tmpy] != null)
                    {
                        if (tileState[tmpx, tmpy].tileClass == 2)
                            obj.surround[i] = tileState[tmpx, tmpy].GetComponent<RotateTile>();
                    }
                    else { obj.surround[i] = null; }
            }

            for (int j = 0; j < 4; j++)
                if (obj.surround[j] != null)
                    RotateCheck(obj.surround[j], !_dir);
        }
    }

    public void UpdateMovableSpace()
    {
        /*
         타일 모양 + Rotation
         - 한쪽만 뚫려있는 모양 0
         ㄴ자모양 1
         ㅏ자모양 2 
         + 자모양 3
         */
        movableSpace = new bool[5 * MAX_BOUND, 5 * MAX_BOUND];
        for (int i = 0; i < MAX_BOUND; i++)
            for (int j = 0; j < MAX_BOUND; j++)
            {
                if (tileState[i, j] != null)
                    if (tileState[i, j].tileClass != 3)
                    {
                        tmp = SerializeTile(tileState[i, j]);
                        for (int x = 0; x < 5; x++)
                            for (int y = 0; y < 5; y++)
                                movableSpace[5 * i + x, 5 * j + y] = tmp[x, y];
                        tmp = new bool[5, 5];
                    }
            }
    }

    public void UpdatePowerCircuit()
    {
        for (int i = 0; i < MAX_BOUND; i++)
            for (int j = 0; j < MAX_BOUND; j++)
            {
                if (tileState[i, j] != null)
                {
                    if (onPowerSpace[2 + 5 * i, 2 + 5 * j])
                        tileState[i, j].isPower = true;
                    else
                        tileState[i, j].isPower = false;

                    if (tileState[i, j].GetComponent<RotateTile>() != null)
                        if (tileState[i, j].GetComponent<RotateTile>().isRotating)
                            tileState[i, j].isPower = false;
                }
            }
        onPowerSpace = new bool[5 * MAX_BOUND, 5 * MAX_BOUND];

        for (int i = 0; i < 4; i++)
        {
            onPowerSpace[powerSource[i, 0], powerSource[i, 1]] = true;

            int temx = 0, temy = 0;
            if (i < 2)
                temx += (int)(Mathf.Pow(-1, i));
            else temy += (int)(Mathf.Pow(-1, i));

            if (powerSource[i, 0] + temx >= 0 && powerSource[i, 0] + temx < 5 * MAX_BOUND && powerSource[i, 1] + temy >= 0 && powerSource[i, 1] + temy < 5 * MAX_BOUND)
                if (movableSpace[powerSource[i, 0] + temx, powerSource[i, 1] + temy])
                    CheckPower(powerSource[i, 0] + temx, powerSource[i, 1] + temy);
        }

        for (int i = 0; i < 4; i++)
            if (onPowerSpace[powerReceiver[i, 0], powerReceiver[i, 1]])
                canEnd = true;
        if (!onPowerSpace[powerReceiver[0, 0], powerReceiver[0, 1]] && !onPowerSpace[powerReceiver[1, 0], powerReceiver[1, 1]] && !onPowerSpace[powerReceiver[2, 0], powerReceiver[2, 1]] && !onPowerSpace[powerReceiver[3, 0], powerReceiver[3, 1]])
            canEnd = false;


    }

    public void CheckPower(int posx, int posy)
    {
        if (!onPowerSpace[posx, posy])
        {
            onPowerSpace[posx, posy] = true;
            for (int i = 0; i < 4; i++)
            {
                int temx = 0, temy = 0;
                if (i < 2)
                    temx += (int)(Mathf.Pow(-1, i));
                else temy += (int)(Mathf.Pow(-1, i));
                if(posx + temx >=  0 && posx + temx < 5*MAX_BOUND && posy + temy >=0 && posy + temy <5*MAX_BOUND)
                    if (movableSpace[posx + temx, posy + temy])
                        CheckPower(posx + temx, posy + temy);
            }
        }
    }

    public void CheckPlayer()
    {
        for (int i = 0; i < MAX_BOUND; i++)
            for (int j = 0; j < MAX_BOUND; j++)
                if (tileState[i, j] != null)
                {
                    if (tileState[i, j].tileClass == 4)
                    {
                        if (tileState[i, j].onPlayer)
                        {
                            if (PlayerController.inst.pposx != tileState[i, j].posx || PlayerController.inst.pposy != tileState[i, j].posy)
                                Destroy(tileState[i, j].gameObject);
                        }
                        else
                        {
                            if (PlayerController.inst.pposx == tileState[i, j].posx && PlayerController.inst.pposy == tileState[i, j].posy)
                                tileState[i, j].onPlayer = true;
                            else tileState[i, j].onPlayer = false;
                        }
                    }
                    else if (tileState[i, j].tileClass == 6)
                    {
                        if (tileState[i, j].onPlayer)
                        {
                            if (PlayerController.inst.pposx != tileState[i, j].posx || PlayerController.inst.pposy != tileState[i, j].posy)
                            {
                                if (tileState[i, j].transform.GetComponent<Door_Tile>() != null)
                                    tileState[i, j].transform.GetComponent<Door_Tile>().isFirst = false;
                                else if (tileState[i, j].transform.GetComponent<Power_Door_Tile>() != null)
                                    tileState[i, j].transform.GetComponent<Power_Door_Tile>().isFirst = false;
                                tileState[i, j].onPlayer = false;
                            }
                        }
                        else
                        {
                            if (PlayerController.inst.pposx == tileState[i, j].posx && PlayerController.inst.pposy == tileState[i, j].posy)
                                tileState[i, j].onPlayer = true;
                            else tileState[i, j].onPlayer = false;
                        }
                    }
                    else
                    {
                        if (PlayerController.inst.pposx == tileState[i, j].posx && PlayerController.inst.pposy == tileState[i, j].posy)
                            tileState[i, j].onPlayer = true;
                        else tileState[i, j].onPlayer = false;
                    }
                }
    }

    public int NormalizeDir(Vector3 dir)
    {
        int result = -1;
        if (dir.normalized.x >= dir.normalized.y && dir.normalized.x > -dir.normalized.y)
        {
            if (tileState[curposx + 1, curposy] == null && curposx != MAX_BOUND - 1)
                result = 2;
        }
        else if (dir.normalized.x < dir.normalized.y && dir.normalized.x >= -dir.normalized.y)
        {
            if (tileState[curposx, curposy + 1] == null && curposy != MAX_BOUND - 1)
                result = 0;
        }
        else if (dir.normalized.x <= dir.normalized.y && dir.normalized.x < -dir.normalized.y)
        {
            if (tileState[curposx - 1, curposy] == null && curposx != 0)
                result = 3;
        }
        else if (dir.normalized.x > dir.normalized.y && dir.normalized.x <= -dir.normalized.y)
        {
            if (tileState[curposx, curposy - 1] == null && curposy != 0)
                result = 1;
        }
        return result;
    }

    public bool[,] SerializeTile(Tile tile)
    {
        bool[,] result = new bool[5, 5];
        if (tile.tileShape == 0)
        {
            bool[,] tmp = new bool[5, 5] {
                { false, false, false, false, false},
                { false, false, false, false, false },
                { false, false, true, true, true },
                { false, false, false, false, false },
                { false, false, false, false, false } };
            result = tmp;
        }
        else if (tile.tileShape == 1)
        {
            bool[,] tmp = new bool[5, 5] {
                { false, false, false, false, false},
                { false, false, false, false, false },
                { false, false, true, true, true },
                { false, false, true, false, false },
                { false, false, true, false, false } };
            result = tmp;
        }
        else if (tile.tileShape == 2)
        {
            bool[,] tmp = new bool[5, 5] {
                { false, false, false, false, false},
                { false, false, false, false, false },
                { true, true, true, true, true },
                { false, false, true, false, false },
                { false, false, true, false, false } };
            result = tmp;
        }
        else if (tile.tileShape == 3)
        {
            bool[,] tmp = new bool[5, 5] {
                { false, false, true, false, false},
                { false, false, true, false, false },
                { true, true, true, true, true },
                { false, false, true, false, false },
                { false, false, true, false, false } };
            result = tmp;
        }
        else if (tile.tileShape == 4)
        {
            bool[,] tmp = new bool[5, 5] {
                { false, false, false, false, false},
                { false, false, false, false, false },
                { true,  true,  true,  true,  true },
                { false, false, false, false, false },
                { false, false, false, false, false } };
            result = tmp;
        }

        if ((tile.transform.eulerAngles.z + 360) % 360 == 270)
            result = spinArray(result);
        else if ((tile.transform.eulerAngles.z + 360) % 360 == 180)
            result = spinArray(spinArray(result));
        else if ((tile.transform.eulerAngles.z + 360) % 360 == 90)
            result = spinArray(spinArray(spinArray(result)));

        if (tile.tileClass == 5)
        {
            bool[,] tmp = new bool[5, 5] {
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true } };
            result = tmp;
        }

        if (tile.tileClass == 6) {
            if (tile.tileShape == 0)
            {
                bool[,] tmp = new bool[5, 5] {
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true } };
                result = tmp;
            }
            else if(canEnd)
            {
                bool[,] tmp = new bool[5, 5] {
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true },
                { true, true, true, true, true } };
                result = tmp;
            }
            else
            {
                bool[,] tmp = new bool[5, 5];
                result = tmp;
            }
        }

        if (tile.tileClass == 3 || tile.tileClass == 7)
        {
            bool[,] tmp = new bool[5, 5];
            result = tmp;
        }

        return result;
    }
    public bool[,] spinArray(bool[,] input)
    {
        bool[,] output = new bool[5, 5];
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                output[j, 4 - i] = input[i, j];
        return output;
    }
}