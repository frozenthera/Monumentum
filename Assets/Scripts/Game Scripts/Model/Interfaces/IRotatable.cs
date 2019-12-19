using System;

namespace Monument.Model
{
    public interface IRotatable
    {
        void RotateTile();
        event Action OnRotated;
    }
}