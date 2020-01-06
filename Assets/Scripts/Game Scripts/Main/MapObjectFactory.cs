using Monumentum.Model;
using Monumentum.Skin;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Monumentum.Controller
{
    public static class MapObjectFactory
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

        public static void OnStartGame()
        {
            MapUtility.OnReset += ResetMap;
            Theme.OnThemeLoaded += CreateAsGameObject;
        }

        private static void ResetMap()
        {
            int count = MapParent.transform.childCount;
            for (int i = 0; i < count; i++)
                Object.Destroy(MapParent.transform.GetChild(i).gameObject);
        }

        public static void CreateAsGameObject(this IBlock block, Sprite sprite)
        {
            GameObject newObject = new GameObject();
            newObject.AddComponent<SpriteRenderer>().sprite = sprite;
            newObject.transform.position = block.Coord.ToVector3();
            newObject.transform.SetParent(MapParent.transform);
            newObject.AddFitComponents(block);
        }
    }
}