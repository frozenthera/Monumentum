using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
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


    }
