using System;

namespace Monument.Model
{
    public interface IRotatable
    {
        void RotateBlock(bool isClockwise = true);
        event Action OnRotated;
    }
}