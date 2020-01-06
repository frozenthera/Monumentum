using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private readonly static Dictionary<BlockType, List<IBlock>> allBlocks = new Dictionary<BlockType, List<IBlock>>();

        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Direction dir, int durablity) => CreateBlock(type, coord, dir, null, durablity);
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, IStage nextStage, Vector2Int nextCoord) => CreateBlock(type, coord, Direction.None, nextStage, -1, nextCoord);
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Direction dir = Direction.None, IStage nextStage = null, int durablity = -1, Vector2Int nextCoord = new Vector2Int())
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
                    newBlock = new Portal(coord, nextStage, nextCoord);
                    break;
                case BlockType.RotateTile:
                    newBlock = new RotatableTile(coord, dir, durablity);
                    break;
            }

            coord.AddBlock(newBlock);
            OnCreated?.Invoke(type, newBlock);
            return newBlock;
        }
        public static event Action<BlockType, IBlock> OnCreated;
        public static event Action<IStage, Vector2Int> OnPortalUsed;
        /*public static Player CreatePlayer(this Vector2Int coord)
        {
            //Player player = new Player(coord);
            return player;
        }*/
    }
}