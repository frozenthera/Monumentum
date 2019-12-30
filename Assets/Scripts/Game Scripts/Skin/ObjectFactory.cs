using Monument.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Monument.Controller
{
    public static class ObjectFactory
    {
        private static GameObject mapParent = null;
        private static GameObject MapParent
        {
            get
            {
                if (mapParent == null)
                    mapParent = new GameObject("Map");
                return mapParent;
            }
        }

        public static void ResetMap()
        {
            int count = MapParent.transform.childCount;
            for (int i = 0; i < count; i++)
                Object.Destroy(MapParent.transform.GetChild(i).gameObject);
        }

        public static GameObject CreateAsGameObject(this IBlock block, Sprite sprite)
        {
            GameObject newObject = new GameObject();
            newObject.AddComponent<SpriteRenderer>().sprite = sprite;
            newObject.transform.position = block.Coord.ToVector3();
            newObject.transform.SetParent(MapParent.transform);

            switch (block)
            {
                case IMovableBlock m:
                    newObject.AddComponent<BlockDragHandler>().Load(m);
                    break;
                case IInteractable i:
                    newObject.AddComponent<PushableHandler>().Load(i);
                    break;
            }

            return newObject;
        }
    }
}