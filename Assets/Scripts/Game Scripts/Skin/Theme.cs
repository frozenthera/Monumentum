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

        private static GameObject CreateBlockGameObject(IBlock block, GameObject gameObject)
        {
            GameObject newObject = GameObject.Instantiate(gameObject, block.Coord.ToVector3(), new Quaternion());
            newObject.GetComponent<BlockController>().Load(block);
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
            private GameObject gameObject;

            public void LoadSet()
            {
                IEnumerable<IBlock> blocks = blockType.GetAllBlocks();
                foreach (var block in blocks)
                    CreateBlockGameObject(block, gameObject);
            }
        }
    }
}