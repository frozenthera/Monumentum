using System;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private class Portal : IInteractable
        {
            public Portal(Vector2Int coord, IStage nextStage)
            {
                this.coord = coord;
                this.nextStage = nextStage;
            }

            private Vector2Int coord;
            private IStage nextStage;

            public Vector2Int Coord => coord;

            public event Action OnInteracted;

            void IInteractable.Interact(ILocatable mob, Direction dir)
            {
                nextStage.ChangeTo();
                OnInteracted?.Invoke();
            }
        }
    }
}