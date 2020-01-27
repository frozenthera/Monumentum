namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private partial class NormalTile
        {
            private class RoadComponent : ITileComponent
            {
                public Directions Directions { get; set; }

                public RoadComponent(Directions directions)
                {
                    Directions = directions;
                }
            }
        }
    }
}