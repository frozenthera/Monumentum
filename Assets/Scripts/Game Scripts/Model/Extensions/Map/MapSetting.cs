using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Model
{
    public static partial class MapSetting
    {
        private readonly static Dictionary<Vector2Int, LocationInfo> locationInfos = new Dictionary<Vector2Int, LocationInfo>();
        private readonly static Dictionary<BlockType, List<IBlock>> allBlocks = new Dictionary<BlockType, List<IBlock>>();

        private static LocationInfo GetLocationInfo(this Vector2Int position)
        {
            locationInfos.TryGetValue(position, out LocationInfo value);
            return value;
        }

        public static IEnumerable<IBlock> GetAllBlocks(this BlockType type)
        {
            return allBlocks.ContainsKey(type) ? allBlocks[type] : null;
        }
        public static IEnumerable<ITile> GetAllTiles(this BlockType type)
        {
            return allBlocks.ContainsKey(type) ? allBlocks[type].Cast<ITile>() : null;
        }

        public static bool IsEmpty(this Vector2Int coord)
        {
            return !locationInfos.ContainsKey(coord);
        }

        public static Vector2Int PlayerCoord { get; private set; }

        #region 생성 메서드
        public static void CreateBlock(this BlockType type, Vector2Int coord, Direction direction = Direction.None, IStage nextStage = null)
        {
            IBlock block = null;
            switch (type)
            {
                case BlockType.Normal:
                    block = new NormalTile(coord, direction);
                    break;
                case BlockType.Wall:
                    block = new Wall(coord);
                    break;
                case BlockType.Portal:
                    block = new Portal(coord, nextStage);
                    break;
            }

            if (block is IMovableBlock movable)
                movable.OnMoved += () => UpdateLoactionInfos(movable);

            if (!locationInfos.ContainsKey(coord))
                locationInfos.Add(coord, new LocationInfo(block, type));
            if (!allBlocks.ContainsKey(type))
                allBlocks[type] = new List<IBlock>();
            allBlocks[type].Add(block);

            void UpdateLoactionInfos(IMovableBlock movable2)
            {
                BlockType type2 = locationInfos[movable2.PreviousCoord].Type;
                locationInfos.Remove(movable2.PreviousCoord);
                locationInfos.Add(movable2.Coord, new LocationInfo(movable2, type2));
            }
        }
        public static void CreateBlock(this BlockType type, IEnumerable<Vector2Int> coords, Direction direction = Direction.None)
        {
            foreach (Vector2Int coord in coords)
            {
                CreateBlock(type, coord, direction);
            }
        }

        public static Player CreatePlayer(this Vector2Int coord)
        {
            Player player = new Player(coord);
            player.OnMoved += () => PlayerCoord = player.Position.ToVector2Int();
            return player;
        }

        #endregion

        #region 길 구하기
        public static bool CanStandOn(this Vector2 position)
        {
            Vector2Int coord = position.ToVector2Int();
            
            LocationInfo location = coord.GetLocationInfo();
            if (location == null) return false;

            ITile tile = location.GetTile();
            if (tile == null) return false;
            
            Direction openDirections = tile.OpenDirections;

            if (openDirections.HasFlag(Direction.Up))
            {
                if (Direction.Up.GetAccessibleSpace(coord).Contains(position))
                    return true;
            }
            if (openDirections.HasFlag(Direction.Down))
                if (Direction.Down.GetAccessibleSpace(coord).Contains(position))
                    return true;
            if (openDirections.HasFlag(Direction.Left))
                if (Direction.Left.GetAccessibleSpace(coord).Contains(position))
                    return true;
            if (openDirections.HasFlag(Direction.Right))
                if (Direction.Right.GetAccessibleSpace(coord).Contains(position))
                    return true;
            return false;
        }
        
        public static bool IsCloseTo(this Vector2Int coord, ILocatable mob)
        {
            const float Length = BlockUnit + 2 * JudgeUnit;
            Rect rect = new Rect(coord.x - Length / 2f, coord.y - Length / 2f, Length, Length);
            return rect.Contains(mob.Position);
        }

        //반드시 낱개의 direction에만 적용하시오.
        private static Rect GetAccessibleSpace(this Direction direction, Vector2Int coord)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathWidth / 2f, PathWidth, PathHeight);
                case Direction.Down:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathHeight, PathWidth, PathHeight);
                case Direction.Left:
                    return new Rect(coord.x - 1 / 2f, coord.y - PathWidth / 2f, PathHeight, PathWidth);
                case Direction.Right:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathWidth / 2f, PathHeight, PathWidth);
                default:
                    return new Rect();
            }
        }
        private const float BlockUnit = 1f;
        private const float JudgeUnit = 1 / 5f;
        private const float PathWidth = JudgeUnit;
        private const float PathHeight = (PathWidth + 1) / 2f;

        #endregion
    }
}