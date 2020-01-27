namespace Monumentum.Model
{


    public static partial class BlockFactory
    {
        private interface IWall : IBlock
        {
            void AddHanging(IWallHanging hanging);
        }
    }
}