using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static partial class MapSetting
    {
        private class LocationInfo
        {
            public LocationInfo(IBlock block, BlockType type = BlockType.None)
            {
                Block = block;
                //Type = type;
            }

            public IBlock Block { get; private set; }
            //public BlockType Type { get; private set; }

            public ITile GetTile()
            {
                return Block as ITile;
            }

            private readonly static Dictionary<Vector2Int, LocationInfo> locationInfos = new Dictionary<Vector2Int, LocationInfo>();
            public static bool IsEmpty(Vector2Int coord)
            {
                return !locationInfos.ContainsKey(coord);
            }
        }
    }
}