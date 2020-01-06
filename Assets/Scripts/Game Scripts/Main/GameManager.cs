using Monumentum.Model;
using Monumentum.Model.Serialized;
using UnityEngine;

namespace Monumentum.Controller.Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player;
        [SerializeField]
        private Stage currentStage;
        [SerializeField]
        private WindowHandler window;
        [SerializeField]
        private KeyManager keyManager;

        void Start()
        {
            GameUtility.Init(player, currentStage);

            window.Init();

            keyManager.OnTryWalk += player.Walk;
            keyManager.OnTryInteract += player.Interact;
            keyManager.Init();

            SaveSystem.OnStartGame();
        }
    }
}