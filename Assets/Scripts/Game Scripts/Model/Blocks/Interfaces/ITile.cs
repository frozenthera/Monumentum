namespace Monumentum.Model
{
    public interface ITile : IBlock
    {
        Direction OpenDirections { get; }
    }
}