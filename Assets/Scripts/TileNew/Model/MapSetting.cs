using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static class MapSetting
    {
        private readonly static Dictionary<Vector2Int, LocationInfo> locationInfos = new Dictionary<Vector2Int, LocationInfo>();

        private static LocationInfo GetLocationInfo(this Vector2Int position)
        {
            locationInfos.TryGetValue(position, out LocationInfo value);
            return value;
        }
        

        public static IBlock CreateBlock(this BlockType type, Vector2Int position, Direction direction = Direction.None)
        {
            IBlock block = null;
            switch (type)
            {
                case BlockType.Normal:
                    block = new NormalTile(direction);
                    break;
            }
            
            if(!locationInfos.ContainsKey(position))
                locationInfos.Add(position, new LocationInfo());
            locationInfos[position].Add(block);
            
            return null;
        }


        #region 형 변환
        public static Vector2Int ToVector2Int(this Vector2 vector2)
        {
            return new Vector2Int((int)(vector2.x + 0.5f), (int)(vector2.y + 0.5f));
        }

        public static Vector3 ToVector3(this Vector2Int vector2)
        {
            return ToVector3((Vector2)vector2);
        }
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return vector2;
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
            LocationInfo location = position.ToVector2Int().GetLocationInfo();
            if (location == null) return false;

            ITile tile = location.GetTile();
            if (tile == null) return false;

            Direction openDirections = tile.OpenDirections;

            if (openDirections.HasFlag(Direction.Up))
                if (Direction.Up.GetAccessibleSpace().Contains(position))
                    return true;
            if (openDirections.HasFlag(Direction.Down))
                if (Direction.Down.GetAccessibleSpace().Contains(position))
                    return true;
            if (openDirections.HasFlag(Direction.Left))
                if (Direction.Left.GetAccessibleSpace().Contains(position))
                    return true;
            if (openDirections.HasFlag(Direction.Right))
                if (Direction.Right.GetAccessibleSpace().Contains(position))
                    return true;

            return false;
        }
        
        //반드시 낱개의 direction에만 적용하시오.
        private static Rect GetAccessibleSpace(this Direction direction, Vector2Int position = new Vector2Int())
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Rect(position.x - PathWidth / 2, position.y + 0.5f, PathWidth, PathHeight);
                case Direction.Down:
                    return new Rect(position.x - PathWidth / 2, position.y + PathWidth / 2, PathWidth, PathHeight);
                case Direction.Left:
                    return new Rect(position.x - 0.5f, position.y, PathHeight, PathWidth);
                case Direction.Right:
                    return new Rect(position.x - PathWidth / 2, position.y, PathHeight, PathWidth);
                default:
                    return new Rect();
            }
        }
        private const float PathWidth = 0.2f;
        private const float PathHeight = (PathWidth + 1) / 2f;
        #endregion

        private class LocationInfo
        {
            private List<IBlock> blocks = new List<IBlock>();
            public ITile GetTile()
            {
                return blocks?[0] as ITile;
            }

            public void Add(IBlock block) => blocks.Add(block);
        }
    }
}