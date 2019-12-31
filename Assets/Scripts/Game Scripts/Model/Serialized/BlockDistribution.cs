using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    public partial class Stage
    {
        [Serializable]
        private class BlockDistribution : IDistribution
        {
            [SerializeField]
            private BlockType blockType;
            [SerializeField]
            private List<Vector2Int> coords;

            void IDistribution.ApplyToMap(MapCallback callback)
            {
                coords.ForEach(c => callback(blockType, blockType.CreateBlock(c)));
            }
        }
    }
}