using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public class Player : IMovable
    {
        public Vector2 Position { get; private set; }

        public event Action OnMoved;

        void IMovable.Move(Vector2 delta)
        {
            Vector2 destination = delta + Position;
            if (destination.CanStandOn())
            {
                Position += delta;
                OnMoved();
            }
        }

        public void TryMovePlayer(Vector2 vector2)
        {
            //Vector2 destination
            Position += vector2;
        }
    }
}