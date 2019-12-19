using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public class NormalTile : ITile, IMovableBlock
    {
        public NormalTile(Vector2Int coord, Direction openDirections)
        {
            this.Coord = coord;
            this.OpenDirections = openDirections;
        }

        public Vector2Int Coord { get; private set; }
        public Vector2Int PreviousCoord { get; private set; }
        public Direction OpenDirections { get; }


        public event Action OnMoved;

        public void MoveBlock(Direction direction)
        {
            Vector2Int destination = Coord + direction.ToVector2();

            if (destination.IsEmpty() && MapSetting.PlayerCoord != Coord)
            {
                PreviousCoord = Coord;
                Coord = destination;
                OnMoved?.Invoke();
            }
        }
    }
}