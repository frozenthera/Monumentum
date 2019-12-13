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

        [Serializable]
        private class BlockDistribution
        {
            [SerializeField]
            private BlockType blockType;
            public BlockType BlockType => blockType;
            [SerializeField]
            private List<Vector2Int> positions;
            public List<Vector2Int> Positions => positions;
        }

        public Dictionary<BlockType, List<Vector2Int>> Distributions => blocks.ToDictionary(d => d.BlockType, d => d.Positions);
    }
}