using System;

namespace Monumentum.Model
{
    public interface IBreakableBlock : IBlock
    {
        void DamageBlock();
        event Action OnBreaking;
    }
}