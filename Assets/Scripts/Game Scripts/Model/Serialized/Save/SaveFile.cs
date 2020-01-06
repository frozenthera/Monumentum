using System;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    public static partial class SaveSystem
    {
        [Serializable]
        private class SaveFile
        {
            public Vector2Int savedCoord = new Vector2Int();
            public Stage currentStage = null;

            public void Print()
            {
                Debug.Log("SavedCoord = " + savedCoord);
                Debug.Log("currentStage = " + currentStage);
            }
        }
    }
}