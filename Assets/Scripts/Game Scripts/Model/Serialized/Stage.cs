using Monument.Controller;
using Monument.Skin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Model.Serialized
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Stage))]
    public partial class Stage : ScriptableObject, IStage
    {
        [SerializeField]
        private Theme theme;

        [SerializeField]
        private BlockDistribution[] blocks;
        [SerializeField]
        private TileDistribution[] tiles;
        [SerializeField]
        private PortalDistribution[] portals;

        private IEnumerable<IDistribution> Distributions => blocks.Union<IDistribution>(tiles).Union(portals);

        public void ChangeTo()
        {
            ObjectFactory.ResetMap();
            Distributions.ToList().ForEach(d => d.ApplyToMap(theme.LoadTheme));
        }

        [Serializable]
        private class TileDistribution : IDistribution
        {
            [SerializeField]
            private BlockType blockType;
            [SerializeField]
            private Direction openDirections;
            [SerializeField]
            private Vector2Int[] coords;

            void IDistribution.ApplyToMap(MapCallback callback)
            {
                coords.ForEach(c => callback?.Invoke(blockType, blockType.CreateBlock(c, openDirections)));
            }
        }

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

        private delegate void MapCallback(BlockType type, IBlock info);
        private interface IDistribution
        {
            void ApplyToMap(MapCallback callback);
        }
    }
}