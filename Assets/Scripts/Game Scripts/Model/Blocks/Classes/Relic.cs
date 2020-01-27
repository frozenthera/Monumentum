using System;
using System.Collections;
using System.Collections.Generic;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private class Relic : IItem
        {
            private readonly Action OnGain;

            public Relic(Action OnGain)
            {
                this.OnGain = OnGain;
            }

            public void GainItem()
            {
                OnGain.Invoke();
            }
        }
    }

    public interface IItem
    {
        void GainItem();
    }

    public enum RelicEffect
    {
        None,
        MoveBlock,
        Reset,
        Button,
        Power,
    }
}