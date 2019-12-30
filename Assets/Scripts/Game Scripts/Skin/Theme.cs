using Monument.Controller;
using Monument.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Skin
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Theme))]
    public partial class Theme : ScriptableObject
    {
        [SerializeField]
        private BlockSet[] blocks;
        [SerializeField]
        private TileSet[] tiles;

        private Dictionary<BlockType, Action<IBlock>> blockCreators;
        private Dictionary<BlockType, Action<IBlock>> BlockCreators
        {
            get
            {
                if(blockCreators == null)
                {
                    blockCreators = ThemeSets.ToDictionary<IThemeSet, BlockType, Action<IBlock>>(o => o.BlockType, o => o.LoadSet);
                }
                return blockCreators;
            }
        }

        private IEnumerable<IThemeSet> ThemeSets => blocks.Union<IThemeSet>(tiles);

        public void LoadTheme(BlockType type, IBlock block)
        {
            if(BlockCreators.TryGetValue(type, out var Create))
                Create?.Invoke(block);
        }
    }
}