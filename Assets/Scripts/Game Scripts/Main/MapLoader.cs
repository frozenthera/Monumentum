using Monumentum.Model;
using Monumentum.Model.Serialized;
using Monumentum.Skin;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monumentum.Controller
{
    [Serializable]
    public class MapLoader
    {
        [SerializeField]
        private Stage currentStage;

        public void LoadStage()
        {
            currentStage.ChangeTo();
        }
    }
}