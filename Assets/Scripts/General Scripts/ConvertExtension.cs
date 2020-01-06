using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static class ConvertExtension
    {
        public static Vector2Int ToVector2Int(this Vector2 vector2)
        {
            return new Vector2Int(GetNearestInt(vector2.x), GetNearestInt(vector2.y));
        }

        public static Vector3 GetTileCenter(this Vector3 position)
        {
            return new Vector3(GetNearestInt(position.x / Offset), GetNearestInt(position.y / Offset)) * Offset;
        }

        public static Vector3 ToVector3(this Vector2Int vector2)
        {
            return ToVector3((Vector2)vector2);
        }

        private const float Offset = 2f;
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return vector2 * Offset;
        }

        private static int GetNearestInt(float f)
        {
            return f >= 0 ? (int)(f + 0.5f) : (int)(f - 0.5f);
        }

        public static Vector2Int ToVector2Int(this Direction direction)
        {
            Vector2Int vector2 = new Vector2Int();
            if (direction.HasFlag(Direction.Up))
                vector2 += Vector2Int.up;
            if (direction.HasFlag(Direction.Down))
                vector2 += Vector2Int.down;
            if (direction.HasFlag(Direction.Left))
                vector2 += Vector2Int.left;
            if (direction.HasFlag(Direction.Right))
                vector2 += Vector2Int.right;

            return vector2;
        }
        public static Vector2 ToVector2(this Direction direction)
        {
            Vector2 vector2 = new Vector2();
            if (direction.HasFlag(Direction.Up))
                vector2 += Vector2.up;
            if (direction.HasFlag(Direction.Down))
                vector2 += Vector2.down;
            if (direction.HasFlag(Direction.Left))
                vector2 += Vector2.left;
            if (direction.HasFlag(Direction.Right))
                vector2 += Vector2.right;

            return vector2;
        }

        public static Vector2 Rotate(this Vector2 vector2, bool isClockwise = true)
        {
            Vector2 center = vector2.ToVector2Int();
            Vector2 distFromCenter = vector2 - center;

            return center + (isClockwise ? new Vector2(distFromCenter.y, - distFromCenter.x) : new Vector2(- distFromCenter.y, distFromCenter.x));
        }

        public static Vector2Int[] GetNearCoords(this Vector2Int coord)
        {
            return new Vector2Int[] { coord + Vector2Int.up, coord + Vector2Int.right, coord + Vector2Int.down, coord + Vector2Int.left };
        }
        
        public static Rect ToRect(this (float w, float h) fs)
        {
            return new Rect(-fs.w/2, -fs.h/2, fs.w, fs.h);
        }

        public static Rect ToSquare(this float f)
        {
            return new Rect(-f / 2, -f / 2, f, f);
        }
    }
}