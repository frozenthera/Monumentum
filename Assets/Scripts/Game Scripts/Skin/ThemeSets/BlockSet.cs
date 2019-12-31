using Monument.Controller;
using Monument.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Skin
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
                /*IEnumerable<IBlock> blocks = blockType.GetAllBlocks();
                foreach (var block in blocks)
                    block.CreateAsGameObject(sprite);*/
            }
        }
    }
}