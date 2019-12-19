using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Tile : MonoBehaviour
{
    [SerializeField]
    protected float offset = 2f;
    [SerializeField]
    public int MAX_BOUND = 10;
    [SerializeField]
    public int tileShape;

    public RotateTile[] surround = new RotateTile[4];
    public GameObject onPower;
    public MapBlockManager mapBlock;

    public bool isMoving = false;
    public bool onPlayer = false;
    public bool isPower = false;

    public int posx, posy;
    public int tileClass;
    
    protected virtual void Start()
    {
        mapBlock = transform.parent.gameObject.GetComponent<MapBlockManager>();
        posx = LevelManager.inst.Cellarize(transform.localPosition.x);
        posy = LevelManager.inst.Cellarize(transform.localPosition.y);
    }

    protected virtual void Update()
    {

    }

    public void MoveTile(int dir)
    {
        if (dir != -1)
        {
            //dir = 0 상 1 하 2 우 3 좌 -> 대응되는 방향
            int cdposx = this.posx, cdposy = this.posy;
            Vector3 shouldMove;

            if (dir < 2)
                cdposy += (int)Mathf.Pow(-1, dir);
            else cdposx += (int)Mathf.Pow(-1, dir);


            mapBlock.tileState[cdposx, cdposy] = mapBlock.tileState[posx, posy];
            mapBlock.tileState[posx, posy] = null;

            if (dir == 0)
                shouldMove = new Vector3(0, 1, 0);
            else if (dir == 1)
                shouldMove = new Vector3(0, -1, 0);
            else if (dir == 2)
                shouldMove = new Vector3(1, 0, 0);
            else shouldMove = new Vector3(-1, 0, 0);

            StartCoroutine(moving(cdposx, cdposy, shouldMove));
            this.posx = cdposx;
            this.posy = cdposy;
            mapBlock.curTarget = null;
        }
    }

    public IEnumerator moving(int cdposx, int cdposy, Vector3 dir)
    {
        isMoving = true;
        float timer = 0;
        while (transform.localPosition != new Vector3(cdposx * offset + offset / 2, cdposy * offset + offset / 2, 0) && timer < 1)
        {
            timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(cdposx * offset + offset / 2, cdposy * offset + offset / 2, 0), timer);
            yield return null;
        }
        isMoving = false;
        mapBlock.UpdateMovableSpace();
    }
}