using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static partial class BlockFactory
    {
        private class RotatableTile : ITile, IMovableBlock, IRotatable
        {
            public RotatableTile(Vector2Int coord, Direction openDirections)
            {
                this.Coord = coord;
                this.OpenDirections = openDirections;
            }

            public Vector2Int Coord { get; set; }
            //public Vector2Int PreviousCoord { get; private set; }
            public Direction OpenDirections { get; private set; }


            public event Action OnMoved;

            public event Action OnRotated;

            void IRotatable.RotateBlock(bool isClockwise = false)
            {
                
            }

            void IMovableBlock.MoveBlock(Direction direction)
            {
                if (this.TryMoveBlock(direction))
                    OnMoved?.Invoke();
            }
        }
    }
}