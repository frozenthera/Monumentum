using System.IO;
using System.Text;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    public static partial class SaveSystem
    {
        private const string DefaultSaveName = "SaveFile";
        private static readonly string CreatePath = Application.dataPath + "/" + "Saves";

        private static SaveFile curSave;
        public static Vector2Int SavedCoord
        {
            get
            {
                return curSave.savedCoord;
            }
        }
        public static Stage SavedStage
        {
            get
            {
                return curSave.currentStage;
            }
        }

        private static void CreateJsonFile(string createPath, string fileName, string jsonData)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
        }

        private static T LoadJsonFile<T>(string loadPath, string fileName)
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }

        public static void OnStartGame()
        {
            BlockFactory.OnPortalUsed += SaveGameData;
        }

        public static void MakeNewSave()
        {
            curSave = new SaveFile();
            string jsonData = JsonUtility.ToJson(curSave);
            CreateJsonFile(CreatePath, DefaultSaveName, jsonData);
        }

        public static void LoadGameData(string saveName = DefaultSaveName)
        {
            curSave = LoadJsonFile<SaveFile>(CreatePath, saveName);
            OnLoad?.Invoke(curSave.currentStage, curSave.savedCoord);
        }
        public static SaveEventHandler OnLoad;
        public delegate void SaveEventHandler(IStage stage, Vector2Int coord);

        public static void SaveGameData(IStage stage, Vector2Int coord)
        {
            curSave.currentStage = (Stage)stage;
            curSave.savedCoord = coord;

            string jsonData = JsonUtility.ToJson(curSave);
            CreateJsonFile(CreatePath, DefaultSaveName, jsonData);
        }
    }
}