using System;

namespace Monumentum.Model
{
    [Flags]
    public enum Direction
    {
        None,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
    }
}