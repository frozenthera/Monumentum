using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTile : Tile
{
    public bool isRotating = false;
    public bool isRotate;
    public bool dir;
    public bool[,] playerPos = new bool[5, 5];

    public float rotateSpeed = 1f;

    protected override void Start()
    {
        onPower = transform.Find("onPower").gameObject;
        onPower.SetActive(false);
        base.Start();
        mapBlock.tileState[posx, posy] = GetComponent<RotateTile>();
        tileClass = 2;
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

    public void Rotate()
    {
        if (onPlayer)
        {
            Vector3 posInTile = PlayerController.inst.transform.localPosition - transform.localPosition + new Vector3(offset/2 , offset/2,0);
            int posx = PlayerController.inst.PathCellarize(posInTile.x);
            int posy = PlayerController.inst.PathCellarize(posInTile.y);
            playerPos[posx, posy] = true;
        }

        if (dir)
        {
            playerPos = mapBlock.spinArray(mapBlock.spinArray(mapBlock.spinArray(playerPos)));
            StartCoroutine(rotating(90));
        }
        else
        {
            playerPos = mapBlock.spinArray(playerPos);
            StartCoroutine(rotating(-90));
        }
    }

    IEnumerator rotating(int radian)
    {
        mapBlock.UpdateMovableSpace();
        mapBlock.UpdatePowerCircuit();
        float timer = 0;
        isRotating = true;
        Quaternion oldRotation = transform.rotation;
        Quaternion tempOldRot = oldRotation;
        transform.Rotate(0, 0, radian);
        Quaternion newRotation = transform.rotation;

        while(timer < 1)
        {
            timer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(oldRotation, newRotation, timer);
            if (onPlayer && radian > 0)
                PlayerController.inst.transform.RotateAround(transform.position, Vector3.forward, Quaternion.Angle(tempOldRot, Quaternion.Slerp(oldRotation, newRotation, timer)));
            else if(onPlayer && radian < 0)
                PlayerController.inst.transform.RotateAround(transform.position, Vector3.forward, -Quaternion.Angle(tempOldRot, Quaternion.Slerp(oldRotation, newRotation, timer)));
            tempOldRot = transform.rotation;
            yield return null;
        }
        isRotating = false;

        if (onPlayer)
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    if (playerPos[i, j])
                    {
                        PlayerController.inst.transform.position = transform.position - new Vector3(offset / 2, offset / 2, 0) + new Vector3(i * offset / 5 + offset / 10, j * offset / 5 + offset / 10, 0);
                        playerPos[i, j] = false;
                    }
        }
        
    }
}
