using System;

namespace Monumentum.Model
{
    [Flags]
    public enum Directions
    {
        None,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
    }

    public enum SoleDir
    {
        None,
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8,
    }
}