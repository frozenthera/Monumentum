using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Model.Serialized
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Stage))]
    public class Stage : ScriptableObject
    {
        [SerializeField]
        private BlockDistribution[] blocks;
        [SerializeField]
        private TileDistribution[] tiles;

        public void ApplyToMap()
        {
            foreach (var b in blocks)
                b.ApplyToMap();
            foreach (var t in tiles)
                t.ApplyToMap();
        }

        [Serializable]
        private class BlockDistribution
        {
            [SerializeField]
            private BlockType blockType;
            [SerializeField]
            private List<Vector2Int> coords;

            public void ApplyToMap()
            {
                blockType.CreateBlock(coords);
            }
        }

        [Serializable]
        private class TileDistribution
        {
            [SerializeField]
            private BlockType blockType;
            [SerializeField]
            private Direction openDirections;
            [SerializeField]
            private Vector2Int[] coords;

            public void ApplyToMap()
            {
                blockType.CreateBlock(coords, openDirections);
            }
        }
    }
}