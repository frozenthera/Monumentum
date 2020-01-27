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
            private Directions openDirections;
            [SerializeField]
            private int durablity;
            [SerializeField]
            private Vector2Int[] coords;

            void IDistribution.ApplyToMap()
            {
                coords.ForEach(c => blockType.CreateBlock(c, openDirections, durablity));
            }
        }

        [Serializable]
        private class RoadDistribution : IDistribution
        {
            [SerializeField]
            private Directions roadDirections;

            [SerializeField]
            private Vector2Int[] coords;

            void IDistribution.ApplyToMap()
            {
                coords.ForEach(c => BlockFactory.CreateRoad(c, roadDirections));
            }
        }

        [Serializable]
        private class DurabilityDistribution : IDistribution
        {
            [SerializeField]
            private int durability;

            [SerializeField]
            private Vector2Int[] coords;

            void IDistribution.ApplyToMap()
            {
                coords.ForEach(c => BlockFactory.SetDurability(c, durability));
            }
        }
    }
}