using Monumentum.Model;
using System.Collections.Generic;
using UnityEngine;
using static Monumentum.Model.MapUtility;
using static Monumentum.Skin.Theme;
using static UnityEngine.Object;

namespace Monumentum.Controller
{
    public static class MapObjectFactory
    {
        private static GameObject mapParent = null;
        private static Transform MapParent
        {
            get
            {
                if (mapParent == null)
                    mapParent = new GameObject("Map");
                return mapParent.transform;
            }
        }

        private static readonly Dictionary<Vector2Int, Transform> blocks = new Dictionary<Vector2Int, Transform>();

        /// <summary>
        /// 게임이 실행될 때 호출될 함수입니다.
        /// </summary>
        public static void OnStartGame()
        {
            OnReset += ResetMap;
            OnThemeBaseLoaded += CreateBlock;
            OnThemeAddOnLoaded += CreateRoad;

            //사용되지 않습니다.
            OnThemeLoaded += CreateAsGameObject;
        }

        #region private 메서드
        /// <summary>
        /// 맵을 초기화합니다.
        /// </summary>
        private static void ResetMap()
        {
            int count = MapParent.childCount;

            for (int i = 0; i < count; i++)
            {
                Destroy(MapParent.GetChild(i).gameObject);
            }
            blocks.Clear();
        }

        /// <summary>
        /// 블록을 생성합니다.
        /// </summary>
        /// <param name="voxelGraphic"></param>
        /// <param name="block"></param>
        private static void CreateBlock(GameObject voxelGraphic, IBlock block)
        {
            GameObject newObject = Instantiate(voxelGraphic);
            newObject.transform.position = block.Coord.ToVector3();
            newObject.transform.SetParent(MapParent);
            newObject.AddFitComponents(block);
            blocks.Add(block.Coord, newObject.transform);
        }

        /// <summary>
        /// 길을 블록에 부착합니다.
        /// </summary>
        /// <param name="voxelPrefab">생성할 길 복셀 프리팹</param>
        /// <param name="coord">길을 부착할 좌표</param>
        private static void CreateRoad(GameObject voxelPrefab, Vector2Int coord)
        {
            GameObject newRoad = Instantiate(voxelPrefab);
            newRoad.transform.SetParent(blocks[coord]);
        }


        /// <summary>
        /// 이 함수는 사용되지 않습니다.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="sprite"></param>
        private static void CreateAsGameObject(this IBlock block, Sprite sprite)
        {
            GameObject newObject = new GameObject();
            newObject.AddComponent<SpriteRenderer>().sprite = sprite;
            newObject.transform.position = block.Coord.ToVector3();
            newObject.transform.SetParent(MapParent.transform);
            newObject.AddFitComponents(block);
        }
        #endregion
    }
}