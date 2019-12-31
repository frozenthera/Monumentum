using Monumentum.Controller;
using Monumentum.Model;
using System;
using System.Collections.Generic;
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

            public void LoadSet(IBlock block)
            {
                block.CreateAsGameObject(sprite);
            }
        }
    }
}