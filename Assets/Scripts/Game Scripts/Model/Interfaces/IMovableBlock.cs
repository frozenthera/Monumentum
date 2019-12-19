using System;
using UnityEngine;

namespace Monument.Model
{
    public interface IMovableBlock : IBlock
    {
        Vector2Int PreviousCoord { get; }
        void MoveBlock(Direction direction);
        event Action OnMoved;
    }
}