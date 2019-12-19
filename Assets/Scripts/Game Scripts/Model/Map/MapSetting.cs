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
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Direction direction = Direction.None)
        {
            IBlock block = null;
            switch (type)
            {
                case BlockType.Normal:
                    block = new NormalTile(coord, direction);
                    break;
            }
            if (block is IMovableBlock movable)
            {
                movable.OnMoved += () => UpdateLoactionInfos(movable);
            }

            if (!locationInfos.ContainsKey(coord))
                locationInfos.Add(coord, new LocationInfo(block, type));
            if (!allBlocks.ContainsKey(type))
                allBlocks[type] = new List<IBlock>();
            allBlocks[type].Add(block);

            return null;

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

        #region 형 변환
        public static Vector2Int ToVector2Int(this Vector2 vector2)
        {
            return new Vector2Int(GetNearestInt(vector2.x), GetNearestInt(vector2.y));

            int GetNearestInt(float f)
            {
                return f >= 0 ? (int)(f + 0.5f) : (int)(f - 0.5f);
            }
        }

        public static Vector2Int ToVector2(this Direction direction)
        {
            Vector2Int vector2 = new Vector2Int();
            if(direction.HasFlag(Direction.Up))
                vector2 += new Vector2Int(0, 1);
            if (direction.HasFlag(Direction.Down))
                vector2 += new Vector2Int(0, -1);
            if (direction.HasFlag(Direction.Left))
                vector2 += new Vector2Int(-1, 0);
            if (direction.HasFlag(Direction.Right))
                vector2 += new Vector2Int(1, 0);

            return vector2;
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
                if (Direction.Up.GetAccessibleSpace(coord).Contains(position))
                    return true;
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
        
        //반드시 낱개의 direction에만 적용하시오.
        private static Rect GetAccessibleSpace(this Direction direction, Vector2Int position)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Rect(position.x - PathWidth / 2, position.y, PathWidth, PathHeight);
                case Direction.Down:
                    return new Rect(position.x - PathWidth / 2, position.y - PathHeight, PathWidth, PathHeight);
                case Direction.Left:
                    return new Rect(position.x - 0.5f, position.y - PathWidth / 2, PathHeight, PathWidth);
                case Direction.Right:
                    return new Rect(position.x - PathWidth / 2, position.y - PathWidth / 2, PathHeight, PathWidth);
                default:
                    return new Rect();
            }
        }
        private const float PathWidth = 0.2f;
        private const float PathHeight = (PathWidth + 1) / 2f;

#endregion
    }
}