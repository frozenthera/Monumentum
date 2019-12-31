using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class MapUtility
    {
        private readonly static Dictionary<Vector2Int, LocationInfo> locationInfos = new Dictionary<Vector2Int, LocationInfo>();

        public static bool IsEmpty(this Vector2Int coord)
        {
            return !locationInfos.ContainsKey(coord);
        }

        public static void MoveBlock(this IBlock block, Vector2Int from, Vector2Int to)
        {
            locationInfos.Remove(from);
            locationInfos.Add(to, new LocationInfo(block));
        }

        public static void AddBlock(this Vector2Int coord, IBlock block)
        {
            if (coord.IsEmpty())
                locationInfos.Add(coord, new LocationInfo(block));
        }

        public static void RemoveBlock(this IBlock block)
        {
            locationInfos.Remove(block.Coord);
        }

        public static bool HasBlock<T>(this Vector2Int coord)
        {
            return !coord.IsEmpty() && locationInfos[coord].Block is T;
        }
        public static bool HasBlock<T>(this Vector2Int coord, out T block)
        {
            block = default;
            if (!coord.IsEmpty() && locationInfos[coord].Block is T castBlock)
            {
                block = castBlock;
                return true;
            }
            return false;
        }
        public static T GetBlock<T>(this Vector2Int coord)
        {
            if (!coord.IsEmpty() && locationInfos[coord].Block is T castBlock)
            {
                return castBlock;
            }
            return default;
        }

        public static void ResetMap()
        {
            locationInfos.Clear();
        }
    }
}