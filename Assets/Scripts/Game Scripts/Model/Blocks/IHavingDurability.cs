namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private interface IHavingDurability
        {
            int Durability { set; }
        }
    }
}