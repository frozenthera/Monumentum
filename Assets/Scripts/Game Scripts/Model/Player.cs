using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public class Player : IWalkableMob
    {
        private Player() { }
        public static Player Create(Vector2Int coord)
        {
            Singleton = new Player();
            Singleton.Position = coord;
            return Singleton;
        }

        public static Player Singleton { get; private set; } = null;

        public Vector2 Position { get; private set; }

        public event RotateEventHandler OnRotated;
        public event Action<Vector2> OnMoved;
        public event Action OnWalked;

        public void Walk(Vector2 delta)
        {
            Vector2 destination = delta + Position;
            
            if (destination.CanStandOn())
            {
                Vector2Int pastCoord = Position.ToVector2Int();
                Vector2Int curCoord = destination.ToVector2Int();

                if (pastCoord != curCoord && pastCoord.HasBlock(out IBreakableBlock b))
                    b.DamageBlock();

                Position = destination;

                OnMoved?.Invoke(Position);
                //OnWalked?.Invoke();
            }
        }

        public void Warp(Vector2Int coord)
        {
            Position = coord;
            OnMoved?.Invoke(Position);
        }

        public void RotatePosition(bool isClockwise = true)
        {
            Position = Position.Rotate(isClockwise);
            OnRotated?.Invoke(isClockwise);
        }
    }
}