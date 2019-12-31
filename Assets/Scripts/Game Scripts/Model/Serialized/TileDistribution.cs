using System;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    public partial class Stage
    {
        [Serializable]
        private class TileDistribution : IDistribution
        {
            [SerializeField]
            private BlockType blockType;
            [SerializeField]
            private Direction openDirections;
            [SerializeField]
            private int durablity;
            [SerializeField]
            private Vector2Int[] coords;

            void IDistribution.ApplyToMap(MapCallback callback)
            {
                coords.ForEach(c => callback?.Invoke(blockType, blockType.CreateBlock(c, openDirections, durablity)));
            }
        }
    }
}