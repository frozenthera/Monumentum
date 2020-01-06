using System;

namespace Monumentum.Model
{
    public delegate void RotateEventHandler(bool isClockwise = true);

    public interface IRotatable
    {
        void RotateBlock(bool isClockwise = true);
        event RotateEventHandler OnRotated;
    }
}