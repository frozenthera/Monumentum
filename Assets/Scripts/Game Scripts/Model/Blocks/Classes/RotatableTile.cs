using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private class RotatableTile : ITile, IMovableBlock, IRotatable
        {
            private int durablity;
            
            public RotatableTile(Vector2Int coord, Direction openDirections, int durablity)
            {
                this.Coord = coord;
                this.OpenDirections = openDirections;
                this.durablity = durablity;
            }

            public Vector2Int Coord { get; set; }
            //public Vector2Int PreviousCoord { get; private set; }
            public Direction OpenDirections { get; private set; }

            public event Action OnMoved;
            public event Action OnRotated;

            void IRotatable.RotateBlock(bool isClockwise)
            {
                if (CanRotate())
                {
                    //캐릭터가 이 위에 올라와 있다면.
                    //회전 방향으로 이동.

                    OpenDirections = OpenDirections.Rotate(isClockwise);
                    OnRotated?.Invoke();
                }
            }

            private bool CanRotate()
            {
                return true;
            }

            void IMovableBlock.MoveBlock(Direction direction)
            {
                if (this.TryMoveBlock(direction))
                    OnMoved?.Invoke();
            }
        }
    }
}