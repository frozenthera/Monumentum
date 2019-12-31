using System;
using UnityEngine;

namespace Monument.Model
{
    public interface IMovableBlock : IBlock
    {
        new Vector2Int Coord { get; set; }
        void MoveBlock(Direction direction);
        event Action OnMoved;
    }
}