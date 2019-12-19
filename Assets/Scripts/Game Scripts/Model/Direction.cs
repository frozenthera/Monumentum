using System;

namespace Monument.Model
{
    [Flags]
    public enum Direction
    {
        None,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
    }
}