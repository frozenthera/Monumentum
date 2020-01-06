using Monumentum.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using Monumentum.Controller;

namespace Monumentum.Skin
{
    public partial class Theme
    {
        [Serializable]
        private class TileSet : IThemeSet
        {
            [SerializeField]
            private BlockType blockType;
            BlockType IThemeSet.BlockType => blockType;

            [SerializeField]
            private Sprite[] sprites = new Sprite[16];

            void IThemeSet.LoadSet(IBlock block)
            {
                ITile tile = (ITile)block;

                int pathCode = (int)tile.OpenDirections == -1 ? 15 : (int)tile.OpenDirections;
                Sprite currentSprite = sprites[pathCode];
                OnThemeLoaded(tile, currentSprite);
            }
        }
    }
}