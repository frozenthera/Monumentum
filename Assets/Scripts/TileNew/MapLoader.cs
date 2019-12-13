using Monument.Model;
using Monument.Model.Serialized;
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
        private ControllerByBlockType[] prefabs;
        [NonSerialized]
        private Dictionary<BlockType, ControllerByBlockType> blockDict;

        public void LoadStage()
        {
            if(blockDict == null)
                blockDict = prefabs.ToDictionary(o => o.BlockType);

            var distributions = currentStage.Distributions;

            foreach (var distribution in distributions) {
                blockDict[distribution.Key].CreateBlocks(distribution.Value);
            }
        }

        [Serializable]
        private class ControllerByBlockType
        {
            [SerializeField]
            private BlockType blockType;
            public BlockType BlockType => blockType;
            [SerializeField]
            private BlockController controller;

            public void CreateBlocks(List<Vector2Int> positions)
            {
                foreach (Vector2Int position in positions)
                {
                    GameObject newBlockControl = GameObject.Instantiate(controller.gameObject, position.ToVector3(), new Quaternion());
                    newBlockControl.GetComponent<BlockController>().Load(blockType.CreateBlock(position, Direction.All));
                }
            }
        }
    }
}