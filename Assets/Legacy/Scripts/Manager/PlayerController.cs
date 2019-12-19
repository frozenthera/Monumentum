using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonBehaviour<PlayerController>
{

    [SerializeField]
    float offset = 2f;
    [SerializeField]
    float roadOffset = 0.05f;

    public float Speed;

    public int pposx, pposy;
    public int pathposx, pathposy;
    public MapBlockManager mapBlock;

    protected Vector2 direction;

    private void Awake()
    {
        if (inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
            SetStatic();
    }

    void Start()
    {
        direction = Vector2.zero;
    }

    void Update()
    {
        pposx = TileCellarize(transform.localPosition.x);
        pposy = TileCellarize(transform.localPosition.y);
        pathposx = PathCellarize(transform.localPosition.x);
        pathposy = PathCellarize(transform.localPosition.y);
        GetInput();
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
        if (mapBlock.tileState[pposx, pposy].GetComponent<RotateTile>() != null)
        {
            if (!mapBlock.tileState[pposx, pposy].GetComponent<RotateTile>().isRotating)
                Move();
        }
        else Move();

    }

    public void Move()
    {
        Vector3 pos = transform.localPosition;
        transform.Translate(direction * Speed * Time.deltaTime);
        Vector3 cdpos = transform.localPosition;
        if (mapBlock != null)
        {
            if (!mapBlock.movableSpace[PathCellarize(cdpos.x), PathCellarize(cdpos.y)])
                transform.localPosition = pos;
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    float tmpx = transform.localPosition.x, tmpy = transform.localPosition.y;
                    if (i < 2)
                        tmpx += roadOffset * Mathf.Pow(-1, i);
                    else tmpy += roadOffset * Mathf.Pow(-1, i);
                    if (!mapBlock.movableSpace[PathCellarize(tmpx), PathCellarize(tmpy)])
                        transform.localPosition = pos;
                }
            }
        }
    }

    public void GetInput()
    {
        Vector2 moveVector;

        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        direction = moveVector;
    }
    public int TileCellarize(float pos)
    {
        return (int)(pos / offset);
    }

    public int PathCellarize(float pos)
    {
        return (int)((pos * 5) / offset + offset/10);
    }
}
