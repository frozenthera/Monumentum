using System;
using UnityEngine;

namespace Monument.Model
{
    public interface IMovable : ILocatable
    {
        void Move(Vector2 vector2);
        event Action OnMoved;
    }
}