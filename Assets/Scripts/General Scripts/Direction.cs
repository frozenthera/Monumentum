using System;

namespace Monumentum.Model
{
    [Flags]
    public enum Directions
    {
        None,
        Forward = 1,
        Right = 2,
        Back = 4,
        Left = 8,
    }

    public enum SoleDir
    {
        None,
        Forward = 1,
        Right = 2,
        Back = 4,
        Left = 8,
    }
}