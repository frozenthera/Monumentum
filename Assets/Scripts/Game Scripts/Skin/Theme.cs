using Monumentum.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monumentum.Skin
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Theme))]
    public partial class Theme : ScriptableObject
    {
        [SerializeField]
        private BlockSet[] blocks;
        [SerializeField]
        private TileSet[] tiles;
        private IEnumerable<IThemeSet> ThemeSets => blocks.Union<IThemeSet>(tiles);

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
            BlockFactory.OnCreated += (t, b) => currentTheme.LoadThemePart(t, b);
        }
        private void LoadThemePart(BlockType type, IBlock block)
        {
            if (BlockCreators.TryGetValue(type, out var Create))
                Create?.Invoke(block);
        }

        private static Theme currentTheme;
        public static void SetCurrentTheme(Theme theme)
        {
            if (currentTheme == null)
                Init();
            currentTheme = theme;
        }

        public static event ThemeLoadHandler OnThemeLoaded;

        public delegate void ThemeLoadHandler(IBlock block, Sprite sprite);
        private delegate void ThemeCreator(IBlock block);
    }
}