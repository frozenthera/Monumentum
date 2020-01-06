using UnityEngine;
using UnityEngine.UI;
using static Monumentum.Model.Serialized.SaveSystem;
using static Monumentum.Controller.Main.GameUtility;

namespace Monumentum.Controller.Main
{
    public class WindowHandler : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;

        public void Init()
        {
            newGameButton.onClick.AddListener(MakeNewGame);
            loadGameButton.onClick.AddListener(LoadPreviousGame);
        }


        private void MakeNewGame()
        {
            Remove();
            StartGame();
            MakeNewSave();
        }

        private void LoadPreviousGame()
        {
            Remove();
            StartGame();
            LoadSaveData();
        }

        private void Remove()
        {
            gameObject.SetActive(false);
        }
    }
}