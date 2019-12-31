using System;

namespace Monumentum.Model
{
    public interface IRotatable
    {
        void RotateBlock(bool isClockwise = true);
        event Action OnRotated;
    }
}