using Monument.Controller;
using Monument.Model;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private static GameObject CreateBlockGameObject(IBlock block, Sprite sprite)
        {
            GameObject newObject = new GameObject();
            newObject.AddComponent<SpriteRenderer>().sprite = sprite;
            newObject.transform.position = block.Coord.ToVector3();
            //newObject.GetComponent<BlockController>().Load(block);
            return newObject;
        }
        

        public void LoadThemeToMap()
        {
            foreach (BlockSet block in blocks)
                block.LoadSet();
            foreach (TileSet tile in tiles)
                tile.LoadSet();
        }

        [Serializable]
        private class BlockSet
        {
            [SerializeField]
            private BlockType blockType;
            [SerializeField]
            private Sprite sprite;

            public void LoadSet()
            {
                IEnumerable<IBlock> blocks = blockType.GetAllBlocks();
                foreach (var block in blocks)
                    CreateBlockGameObject(block, sprite);
            }
        }
    }
}