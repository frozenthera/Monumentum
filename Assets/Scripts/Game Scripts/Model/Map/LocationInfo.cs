using System.Collections.Generic;

namespace Monument.Model
{
    public static partial class MapSetting
    {
        private class LocationInfo
        {
            public LocationInfo(IBlock block, BlockType type)
            {
                Block = block;
                Type = type;
            }

            public IBlock Block { get; private set; }
            public BlockType Type { get; private set; }

            public ITile GetTile()
            {
                return Block as ITile;
            }
        }
    }
}