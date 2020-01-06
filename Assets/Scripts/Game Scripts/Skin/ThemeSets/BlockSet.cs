using Monumentum.Model;
using System;
using UnityEngine;

namespace Monumentum.Skin
{
    public partial class Theme
    {
        [Serializable]
        private class BlockSet : IThemeSet
        {
            [SerializeField]
            private BlockType blockType;
            public BlockType BlockType => blockType;
            [SerializeField]
            private Sprite sprite;

            void IThemeSet.LoadSet(IBlock block)
            {
                OnThemeLoaded(block, sprite);
            }
        }
    }
}