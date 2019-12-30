using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public class Wall : IBlock
    {
        public Wall(Vector2Int coord) {
            Coord = coord;
        }

        public Vector2Int Coord { get; private set; }
    }
}