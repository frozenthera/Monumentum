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
            //[SerializeField]
            //private Sprite sprite;
            [SerializeField]
            private GameObject voxelPrefab;

            void IThemeSet.LoadSet(IBlock block)
            {
                //OnThemeLoaded(block, sprite);
                OnThemeBaseLoaded.Invoke(voxelPrefab, block);
            }
        }
    }
}