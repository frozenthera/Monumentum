using Monumentum.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Monumentum.Model.BlockFactory;

namespace Monumentum.Skin
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Theme))]
    public partial class Theme : ScriptableObject
    {
        [SerializeField]
        private BlockSet[] blocks;
        [SerializeField]
        private RoadThemeElement roads;
        private static Theme currentTheme;

        private IEnumerable<IThemeSet> ThemeSets => blocks;//.Union<IThemeSet>(tiles);

        private Dictionary<BlockType, ThemeCreator> blockCreators;
        private Dictionary<BlockType, ThemeCreator> BlockCreators
        {
            get
            {
                if(blockCreators == null)
                    blockCreators = ThemeSets.ToDictionary<IThemeSet, BlockType, ThemeCreator>(o => o.BlockType, o => o.LoadSet);
                return blockCreators;
            }
        }

        private static void Init()
        {
            OnBaseCreated += (t, b) => currentTheme.LoadBaseThemeElement(t, b);
            OnRoadCreated += (c, d) => currentTheme.roads.LoadElement(c, d);
        }

        private void LoadBaseThemeElement(BlockType type, IBlock block)
        {
            if (BlockCreators.TryGetValue(type, out var Create))
                Create?.Invoke(block);
        }

        
        public static void SetCurrentTheme(Theme theme)
        {
            if (currentTheme == null)
                Init();
            currentTheme = theme;
        }

        
        public static event ThemeElementLoadEventHandler OnThemeBaseLoaded;
        public static event ThemeAddOnLoadEventHandler OnThemeAddOnLoaded;
        
        public delegate void ThemeElementLoadEventHandler(GameObject voxelPrefab, IBlock block);
        public delegate void ThemeAddOnLoadEventHandler(GameObject voxelPrefab, Vector2Int coord);

        private delegate void ThemeCreator(IBlock block);

        [System.Serializable]
        private class RoadThemeElement
        {
            /// <summary>
            /// 길의 방향
            /// </summary>
            [SerializeField]
            private GameObject upVoxelPrefab;
            [SerializeField]
            private GameObject rightVoxelPrefab;
            [SerializeField]
            private GameObject downVoxelPrefab;
            [SerializeField]
            private GameObject leftVoxelPrefab;

            public void LoadElement(Vector2Int coord, SoleDir direction)
            {
                switch (direction)
                {
                    case SoleDir.Up:
                        OnThemeAddOnLoaded.Invoke(upVoxelPrefab, coord);
                        return;
                    case SoleDir.Right:
                        OnThemeAddOnLoaded.Invoke(rightVoxelPrefab, coord);
                        return;
                    case SoleDir.Down:
                        OnThemeAddOnLoaded.Invoke(downVoxelPrefab, coord);
                        return;
                    case SoleDir.Left:
                        OnThemeAddOnLoaded.Invoke(leftVoxelPrefab, coord);
                        return;
                }
            }
        }

        /// <summary>
        /// 이하는 사용되지 않습니다.
        /// </summary>
        public static event ThemeLoadHandler OnThemeLoaded;
        public delegate void ThemeLoadHandler(IBlock block, Sprite sprite);
    }
}