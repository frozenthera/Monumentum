using System;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private partial class NormalTile
        {
            private class BreakableComponent : ITileComponent, IBreakableBlock
            {
                private readonly IBlock block;
                private int durability;

                public BreakableComponent(IBlock block, int durability)
                {
                    this.block = block;
                    this.durability = durability;
                }

                public event Action OnBreaking;
                
                public void DamageBlock()
                {
                    if (durability > 0)
                    {
                        durability--;
                        if (durability == 0)
                        {
                            block.RemoveBlock();
                            OnBreaking?.Invoke();
                        }
                    }
                }
            }
        }
    }
}