using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private readonly static Dictionary<BlockType, List<IBlock>> allBlocks = new Dictionary<BlockType, List<IBlock>>();

        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Direction dir, int durablity) => CreateBlock(type, coord, dir, null, durablity);
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, IStage nextStage) => CreateBlock(type, coord, Direction.None, nextStage);
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Direction dir = Direction.None, IStage nextStage = null, int durablity = -1)
        {
            IBlock newBlock = null;
            switch (type)
            {
                case BlockType.Normal:
                    newBlock = new NormalTile(coord, dir, durablity);
                    break;
                case BlockType.Wall:
                    newBlock = new Wall(coord, dir);
                    break;
                case BlockType.Portal:
                    newBlock = new Portal(coord, nextStage);
                    break;
                case BlockType.RotateTile:
                    newBlock = new RotatableTile(coord, dir, durablity);
                    break;
            }

            coord.AddBlock(newBlock);
            return newBlock;
        }

        public static Player CreatePlayer(this Vector2Int coord)
        {
            Player player = new Player(coord);
            return player;
        }
    }
}