using System;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private class Portal : IBlock, IInteractable
        {
            public Portal(Vector2Int coord, IStage nextStage, Vector2Int nextCoord, bool isLocked = false)
            {
                this.coord = coord;
                this.nextStage = nextStage;
                this.nextCoord = nextCoord;
                this.isLocked = isLocked;
            }

            private Vector2Int coord;
            private readonly IStage nextStage;
            private Vector2Int nextCoord;
            private bool isLocked;

            public Vector2Int Coord => coord;

            public event Action<Directions> OnInteracted;

            void IInteractable.Interact(ILocatable mob, Directions dir)
            {
                if (!isLocked || IsPortalUnlocked)
                {
                    nextStage.Generate();
                    OnInteracted?.Invoke(dir);
                    OnPortalUsed?.Invoke(nextStage, nextCoord);
                    mob.Warp(nextCoord);
                }
            }
        }
    }
}