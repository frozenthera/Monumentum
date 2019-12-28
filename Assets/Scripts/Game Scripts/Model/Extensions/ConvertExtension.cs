using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static class ConvertExtension
    {
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
            if (direction.HasFlag(Direction.Up))
                vector2 += new Vector2Int(0, 1);
            if (direction.HasFlag(Direction.Down))
                vector2 += new Vector2Int(0, -1);
            if (direction.HasFlag(Direction.Left))
                vector2 += new Vector2Int(-1, 0);
            if (direction.HasFlag(Direction.Right))
                vector2 += new Vector2Int(1, 0);

            return vector2;
        }
    }
}