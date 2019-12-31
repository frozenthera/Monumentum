using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static partial class BlockFactory
    {
        private class NormalTile : IBreakableBlock, IMovableBlock, ITile
        {
            public NormalTile(Vector2Int coord, Direction openDirections, int durablity = -1)
            {
                this.Coord = coord;
                this.OpenDirections = openDirections;
                this.durablity = durablity;
            }

            private int durablity;

            public Vector2Int Coord { get; set; }
            //public Vector2Int PreviousCoord { get; private set; }
            public Direction OpenDirections { get; }


            void IMovableBlock.MoveBlock(Direction direction)
            {
                if (this.TryMoveBlock(direction))
                    OnMoved?.Invoke();
            }

            void IBreakableBlock.DamageBlock()
            {
                if (durablity > 0)
                {
                    Debug.Log(1);
                    durablity--;
                    if(durablity == 0)
                    {
                        this.RemoveBlock();
                        OnBreaking?.Invoke();
                    }
                }
            }

            public event Action OnMoved;
            public event Action OnBreaking;
        }
    }

    public interface IBreakableBlock : IBlock
    {
        void DamageBlock();
        event Action OnBreaking;
    }
}