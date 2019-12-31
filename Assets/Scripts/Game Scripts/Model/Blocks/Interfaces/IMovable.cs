using System;
using UnityEngine;

namespace Monumentum.Model
{
    public interface IMovableMob : ILocatable
    {
        void Move(Vector2 vector2);
        event Action OnMoved;
    }
}