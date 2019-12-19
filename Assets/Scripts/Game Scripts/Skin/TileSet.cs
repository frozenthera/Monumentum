using Monument.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Skin
{
    public partial class Theme
    {
        [Serializable]
        private class TileSet
        {
            
            [SerializeField]
            private BlockType blockType;
            [Tooltip("위쪽 길이 열린 것을 사용하십시오.")]
            [SerializeField]
            private GameObject oneWayTile;
            [Tooltip("위아래로 열린 것을 사용하십시오.")]
            [SerializeField]
            private GameObject straightWayTile;
            [Tooltip("위와 오른쪽이 열린 것을 사용하십시오.")]
            [SerializeField]
            private GameObject curvedWayTile;
            [Tooltip("왼쪽만 막힌 것을 사용하십시오.")]
            [SerializeField]
            private GameObject tripleWayTile;
            [SerializeField]
            private GameObject crossWayTile;

            public void LoadSet()
            {
                IEnumerable<ITile> tiles = blockType.GetAllTiles();
                foreach (var tile in tiles)
                {
                    int pathCount = tile.OpenDirections.GetFlagCount();
                    
                    switch (pathCount)
                    {
                        case 1:
                            CreateBlockGameObject(tile, oneWayTile);
                            break;
                        case 2:
                            CreateBlockGameObject(tile, straightWayTile);
                            break;
                        case 3:
                            break;
                        case 4:
                            CreateBlockGameObject(tile, crossWayTile);
                            break;
                    }
                }
            }

            private static void CreateTileGameObject(ITile tile, GameObject gameObject)
            {
                GameObject newObject = CreateBlockGameObject(tile, gameObject);
                newObject.transform.rotation = GetQuaternion(tile.OpenDirections);

                Quaternion GetQuaternion(Direction direction)
                {
                    Quaternion RightRotate90 = Quaternion.Euler(0, 0, -90f);
                    Quaternion LeftRotate90 = Quaternion.Euler(0, 0, 90f);
                    Quaternion RightRotate180 = Quaternion.Euler(0, 0, 180f);

                    switch (direction)
                    {
                        case Direction.Up:
                            return Quaternion.identity;
                        case Direction.Left:
                            return LeftRotate90;
                        case Direction.Right:
                            return RightRotate90;
                        case Direction.Down:
                            return RightRotate180;
                        case Direction.Up | Direction.Down:
                            return Quaternion.identity;
                        case Direction.Left | Direction.Right:
                            return RightRotate90;
                        default:
                            return Quaternion.identity;
                    }
                }
            }
        }
    }
}