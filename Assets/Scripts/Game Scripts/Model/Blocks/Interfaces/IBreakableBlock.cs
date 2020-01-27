using System;

namespace Monumentum.Model
{
    public interface IBreakableBlock
    {
        void DamageBlock();
        event Action OnBreaking;
    }
}