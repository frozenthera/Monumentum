using System;
using UnityEngine;

namespace Monumentum.Model
{
    public interface ILocatable
    {
        Vector2 Position { get; }
        void RotatePosition(bool isClockwise = true);
        void Warp(Vector2Int coord);

        event Action<Vector2> OnMoved;
        event RotateEventHandler OnRotated;
    }
}