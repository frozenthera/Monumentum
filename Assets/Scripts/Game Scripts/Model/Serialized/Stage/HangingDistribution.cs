using System;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    public partial class Stage
    {
        [Serializable]
        private class HangingDistribution : IDistribution
        {
            [SerializeField]
            private Vector2Int coord;
            [SerializeField]
            private HangingType hanging;
            [SerializeField]
            private Directions directions;

            void IDistribution.ApplyToMap()
            {
                hanging.CreateHanging(coord, directions);
            }
        }
    }
}


