namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private interface IRoadLayable
        {
            Directions OpenDirections { set; }
        }
    }
}