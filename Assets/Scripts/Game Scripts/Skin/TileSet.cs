using Monument.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using Monument.Controller;

namespace Monument.Skin
{
    public partial class Theme
    {
        [Serializable]
        private class TileSet
        {
            [SerializeField]
            private BlockType blockType;

            [SerializeField]
            private Sprite[] sprites = new Sprite[16];

            public void LoadSet()
            {
                IEnumerable<ITile> tiles = blockType.GetAllTiles();
                foreach (var tile in tiles)
                {
                    int pathCode = (int)tile.OpenDirections == -1 ? 15 : (int)tile.OpenDirections;

                    Sprite currentSprite = sprites[pathCode];
                    CreateTileGameObject(tile, currentSprite);
                }
            }
            private static void CreateTileGameObject(ITile tile, Sprite sprite)
            {
                GameObject newObejct = new GameObject();
                newObejct.transform.position = tile.Coord.ToVector3();
                newObejct.AddComponent<SpriteRenderer>().sprite = sprite;
                if (tile is IMovableBlock movable)
                    newObejct.AddComponent<BlockDragHandler>().Load(movable);
            }
        }
    }
}