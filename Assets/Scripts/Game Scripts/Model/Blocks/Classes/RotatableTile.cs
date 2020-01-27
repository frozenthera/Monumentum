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
            
            public RotatableTile(Vector2Int coord, Directions openDirections, int durablity)
            {
                this.Coord = coord;
                this.OpenDirections = openDirections;
                this.durablity = durablity;
            }

            public Vector2Int Coord { get; set; }
            //public Vector2Int PreviousCoord { get; private set; }
            public Directions OpenDirections { get; private set; }

            public event Action OnMoved;
            public event RotateEventHandler OnRotated;

            void IRotatable.RotateBlock(bool isClockwise)
            {
                if (CanRotate())
                {
                    if (Coord.LocatesPlayer())
                        Player.Singleton.RotatePosition(isClockwise);

                    OpenDirections = OpenDirections.Rotate(isClockwise);
                    OnRotated?.Invoke(isClockwise);
                }
            }

            private bool CanRotate()
            {
                return true;
            }

            void IMovableBlock.MoveBlock(Directions direction)
            {
                if (this.TryMoveBlock(direction))
                    OnMoved?.Invoke();
            }
        }
    }
}