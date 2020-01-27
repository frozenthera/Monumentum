using System;
using UnityEngine;

namespace Monumentum.Model
{
    public interface IMovableBlock : IBlock
    {
        new Vector2Int Coord { get; set; }
        void MoveBlock(Directions direction);
        event Action OnMoved;
    }
}