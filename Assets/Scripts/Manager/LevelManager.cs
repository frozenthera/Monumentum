using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    [SerializeField]
    float offset = 2f;

    public int MAX_BOUND = 10;

    public Camera Camera;
    public GameObject player;
    public List<MapBlockManager> mapList;

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
        player = GameObject.Find("Player");
        Camera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    void Update()
    {

    }

    #region 자료형 반환

    public int Cellarize(float pos)
    {
        return (int)(pos / offset);
    }

    public int CheckAd(GameObject A, GameObject B)
    {
        int distance;
        distance = Mathf.Abs(Cellarize(A.transform.localPosition.x) - Cellarize(B.transform.localPosition.x)) + Mathf.Abs(Cellarize(A.transform.localPosition.y) - Cellarize(B.transform.localPosition.y));
        return distance;
    }

    public int PathCellarize(float pos)
    {
        return (int)((pos * 5) / offset);
    }
}
#endregion
