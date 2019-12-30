namespace Monument.Model
{
    public interface ITile : IBlock
    {
        Direction OpenDirections { get; }
    }
}