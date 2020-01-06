using Monumentum.Model;
using Monumentum.Model.Serialized;

namespace Monumentum.Controller.Main
{
    public static class GameUtility
    {
        private static PlayerController playerController;
        private static IStage curStage;

        public static void Init(PlayerController player, IStage stage)
        {
            playerController = player;
            curStage = stage;
            SaveSystem.OnLoad += (s, c) => { s.ChangeTo(); playerController.Warp(c); };
        }

        public static void StartGame()
        {
            MapObjectFactory.OnStartGame();
            playerController.OnStartGame();
            SaveSystem.OnStartGame();
            //curStage.ChangeTo();
        }

        public static void DoReset()
        {
            playerController.Warp(SaveSystem.SavedCoord);
            curStage.ChangeTo();
        }
    }
}