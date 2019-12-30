using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static partial class BlockFactory
    {
        private class NormalTile : ITile, IMovableBlock
        {
            public NormalTile(Vector2Int coord, Direction openDirections)
            {
                this.Coord = coord;
                this.OpenDirections = openDirections;
            }

            public Vector2Int Coord { get; set; }
            //public Vector2Int PreviousCoord { get; private set; }
            public Direction OpenDirections { get; }


            public event Action OnMoved;

            public void MoveBlock(Direction direction)
            {
                if (this.TryMoveBlock(direction))
                    OnMoved?.Invoke();
            }
        }
    }
}