using System;

namespace Monument.Model
{
    public interface IRotatable
    {
        void RotateBlock(bool isClockwise = false);
        event Action OnRotated;
    }
}