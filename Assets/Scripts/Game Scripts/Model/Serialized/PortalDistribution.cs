using System;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    public partial class Stage
    {
        [Serializable]
        private class PortalDistribution : IDistribution
        {
            [SerializeField]
            private Vector2Int coord;
            [SerializeField]
            private Stage nextMap;
            [SerializeField]
            private Vector2Int nextCoord;

            void IDistribution.ApplyToMap(MapCallback callback)
            {
                callback(BlockType.Portal, BlockType.Portal.CreateBlock(coord, nextMap));
            }
        }
    }
}