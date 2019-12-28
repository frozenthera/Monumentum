using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public class Player : IMovableMob
    {
        public Player(Vector2Int coord)
        {
            Position = coord;
            Singleton = this;
        }

        public static Player Singleton { get; private set; }

        public Vector2 Position { get; private set; }

        public event Action OnMoved;

        void IMovableMob.Move(Vector2 delta)
        {
            Vector2 destination = delta + Position;
            if (destination.CanStandOn())
            {
                Position += delta;
                OnMoved();
            }
        }
    }
}