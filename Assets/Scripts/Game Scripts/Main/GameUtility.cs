using Monumentum.Model;
using Monumentum.Model.Serialized;
using UnityEngine;

namespace Monumentum.Controller.Main
{
    public static class GameUtility
    {
        private static PlayerController playerController;
        private static IStage curStage;
        private static Vector2Int savedCoord;

        public static void Init(PlayerController player, IStage stage)
        {
            playerController = player;
            curStage = stage;
            SaveSystem.OnLoad += (s, c) => { curStage = s; savedCoord = c; };
        }

        public static void StartGame()
        {
            MapObjectFactory.OnStartGame();
            playerController.OnStartGame();
            SaveSystem.OnStartGame();

            DoReset();
        }

        public static void DoReset()
        {
            playerController.Warp(SaveSystem.SavedCoord);
            curStage.Generate();
        }
    }
}