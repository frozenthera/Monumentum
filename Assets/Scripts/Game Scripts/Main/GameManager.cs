using System.Collections;
using UnityEngine;

namespace Monument.Controller
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player;
        [SerializeField]
        private MapLoader map;
        void Start()
        {
            player.Init();
            map.LoadStage();
        }
    }
}