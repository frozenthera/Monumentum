using Monument.Skin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Model.Serialized
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Stage))]
    public class Stage : ScriptableObject, IStage
    {
        [SerializeField]
        private Theme theme;

        [SerializeField]
        private BlockDistribution[] blocks;
        [SerializeField]
        private TileDistribution[] tiles;
        [SerializeField]
        private PortalDistribution[] portals;

        public void ChangeStage()
        {
            foreach (var b in blocks)
                b.ApplyToMap();
            foreach (var t in tiles)
                t.ApplyToMap();
            foreach (var p in portals)
                p.ApplyToMap();

            theme.LoadThemeToMap();
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

        [Serializable]
        private class PortalDistribution
        {
            [SerializeField]
            private Vector2Int coord;
            [SerializeField]
            private Stage nextMap;
            [SerializeField]
            private Vector2Int nextCoord;

            public void ApplyToMap()
            {
                BlockType.Portal.CreateBlock(coord);
            }
        }
    }
}