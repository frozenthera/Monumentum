using Monument.Model;
using Monument.Model.Serialized;
using Monument.Skin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Controller
{
    [Serializable]
    public class MapLoader
    {
        [SerializeField]
        private Stage currentStage;

        [SerializeField]
        private Theme currentTheme;

        public void LoadStage()
        {
            currentStage.ApplyToMap();
            currentTheme.LoadThemeToMap();
        }
    }
}