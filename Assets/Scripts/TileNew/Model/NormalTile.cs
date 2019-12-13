using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public class NormalTile : ITile, IMovableTile
    {
        public NormalTile(Direction openDirections)
        {
            this.openDirections = openDirections;
        }

        public Vector2Int Position { get; private set; }

        private readonly Direction openDirections;
        public Direction OpenDirections => openDirections;


        public event Action OnMoved;

        public void MoveTile(Direction direction)
        {
            Position += direction.ToVector2();
        }
    }
}