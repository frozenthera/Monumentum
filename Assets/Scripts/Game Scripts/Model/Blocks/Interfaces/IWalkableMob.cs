using System;
using UnityEngine;

namespace Monumentum.Model
{
    public interface IWalkableMob : ILocatable
    {
        void Walk(Vector2 vector2);
        event Action OnWalked;
    }
}