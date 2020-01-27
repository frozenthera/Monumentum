namespace Monumentum.Model
{
    public interface ITile : IBlock
    {
        Directions OpenDirections { get; }
    }
}