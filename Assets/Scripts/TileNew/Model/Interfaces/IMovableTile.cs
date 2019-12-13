using System;
using UnityEngine;

namespace Monument.Model
{
    public interface ILocatable
    {
        Vector2 Position { get; }
    }

    public interface IMovable : ILocatable
    {
        void Move(Vector2 vector2);
        event Action OnMoved;
    }

    public interface IMovableTile
    {
        void MoveTile(Direction direction);
        event Action OnMoved;
    }
}