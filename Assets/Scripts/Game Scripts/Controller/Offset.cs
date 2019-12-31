using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Controller
{
    public static class Offset
    {
        public static Vector3 ToVector3(this Vector2Int vector2)
        {
            return ToVector3((Vector2)vector2);
        }
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return vector2 * 2;
        }
    }
}