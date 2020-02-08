using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public static class BlockUtility
    {
        public static bool HasBlock(this Vector3 pos)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Block") + 1 << LayerMask.NameToLayer("Path");
            return Physics.Raycast(pos - 3* Vector3.up, Vector3.up, 3f, layerMask);
        }

        public static bool HasBlock<T>(this Vector3 pos, out T result) where T : MonoBehaviour
        {
            int layerMask = 1 << LayerMask.NameToLayer("Block");
            Physics.Raycast(pos - Vector3.up, Vector3.up, out var hitInfo, 3f, layerMask);
            
            result = hitInfo.transform?.GetComponent<T>();

            return result != null;
        }

        public static Vector3[] GetNearPositions(this Vector3 pos)
        {
            return new Vector3[] { pos + Vector3.up, pos + Vector3.right, pos + Vector3.down, pos + Vector3.left, };
        }
    }
}