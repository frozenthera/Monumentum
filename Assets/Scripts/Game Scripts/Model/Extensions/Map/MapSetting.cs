using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Model
{
    public static partial class MapSetting
    {
        private readonly static Dictionary<Vector2Int, LocationInfo> locationInfos = new Dictionary<Vector2Int, LocationInfo>();
        
        private static LocationInfo GetLocationInfo(this Vector2Int position)
        {
            locationInfos.TryGetValue(position, out LocationInfo value);
            return value;
        }

        public static bool IsEmpty(this Vector2Int coord)
        {
            return !locationInfos.ContainsKey(coord);
        }

        private static void MoveBlock(this IBlock block, Vector2Int from, Vector2Int to)
        {
            locationInfos.Remove(from);
            locationInfos.Add(to, new LocationInfo(block));
        }

        public static void AddBlock(this Vector2Int coord, IBlock block)
        {
            if (coord.IsEmpty())
                locationInfos.Add(coord, new LocationInfo(block));
        }

        public static bool HasBlock<T>(this Vector2Int coord, out T block)
        {
            block = default;
            if(!coord.IsEmpty() && locationInfos[coord].Block is T castBlock)
            {
                block = castBlock;
                return true;
            }
            return false;
        }

        #region 길 구하기
        
        public static bool TryMoveBlock(this IMovableBlock block, Direction direction)
        {
            Vector2Int destination = block.Coord + direction.ToVector2();

            if (destination.IsEmpty() && Player.Singleton.Position.ToVector2Int() != block.Coord)
            {
                block.MoveBlock(block.Coord, destination);
                block.Coord = destination;
                return true;
            }
            return false;
        }

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
        
        public static void TryInteract(this ILocatable mob)
        {
            if (TryGetExpectedCoord(mob.Position, out Vector2Int coord))
            {
                
                if (coord.HasBlock(out IInteractable pushable))
                    pushable.Interact(mob);
            }

            bool TryGetExpectedCoord(Vector2 position, out Vector2Int expectedCoord)
            {
                
                Vector2 distFromCenter = position - position.ToVector2Int();
                expectedCoord = position.ToVector2Int();
                Debug.Log(distFromCenter);
                Debug.Log(GetRect(Direction.Down));
                switch (distFromCenter)
                {
                    case var d when (GetRect(Direction.Up).Contains(d)):
                        expectedCoord += Vector2Int.up;
                        return true;
                    case var d when (GetRect(Direction.Down).Contains(d)):
                        expectedCoord += Vector2Int.down;
                        return true;
                    case var d when (GetRect(Direction.Left).Contains(d)):
                        expectedCoord += Vector2Int.left;
                        return true;
                    case var d when (GetRect(Direction.Right).Contains(d)):
                        expectedCoord += Vector2Int.right;
                        return true;
                }
                
                return false;

                Rect GetRect(Direction dir)
                {
                    Rect rect = InteractRect;
                    switch (dir)
                    {
                        case Direction.Up:
                            rect.center += new Vector2(0, JudgeUnit * 2);
                            break;
                        case Direction.Down:
                            rect.position -= new Vector2(JudgeUnit / 2f, JudgeUnit * 5 / 2);
                            break;
                        case Direction.Left:
                            rect.center -= new Vector2(JudgeUnit * 2, 0);
                            break;
                        case Direction.Right:
                            rect.center += new Vector2(JudgeUnit * 2, 0);
                            break;
                    }
                    return rect;
                }
            }
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

        private static readonly Rect InteractRect = new Rect(0, 0, JudgeUnit, JudgeUnit);

        private const float BlockUnit = 1f;
        private const float JudgeUnit = 1 / 5f;
        private const float PathWidth = JudgeUnit;
        private const float PathHeight = (PathWidth + 1) / 2f;

        #endregion
    }
}