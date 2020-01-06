using System;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private class Portal : IInteractable
        {
            public Portal(Vector2Int coord, IStage nextStage, Vector2Int nextCoord)
            {
                this.coord = coord;
                this.nextStage = nextStage;
                this.nextCoord = nextCoord;
            }

            private Vector2Int coord;
            private IStage nextStage;
            private Vector2Int nextCoord;

            public Vector2Int Coord => coord;

            public event Action OnInteracted;

            void IInteractable.Interact(ILocatable mob, Direction dir)
            {
                nextStage.ChangeTo();
                OnInteracted?.Invoke();
                OnPortalUsed?.Invoke(nextStage, nextCoord);
                mob.Warp(nextCoord);
            }
        }
    }
}