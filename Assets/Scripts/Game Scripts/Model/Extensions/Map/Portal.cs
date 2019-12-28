using System;
using UnityEngine;

namespace Monument.Model
{
    public static partial class MapSetting
    {
        private class Portal : IPushable
        {
            public Portal(Vector2Int coord, IStage nextStage)
            {
                this.coord = coord;
                this.nextStage = nextStage;
            }

            private Vector2Int coord;
            private IStage nextStage;

            public Vector2Int Coord => coord;

            public event Action OnPushed;

            public void Push(ILocatable mob)
            {
                if (coord.IsCloseTo(mob))
                {
                    nextStage.ChangeStage();
                    OnPushed?.Invoke();
                }
            }
        }
    }

    public interface IPushable : IBlock
    {
        void Push(ILocatable mob);
        event Action OnPushed;
    }

    public interface IStage
    {
        void ChangeStage();
    }
}