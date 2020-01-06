using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public class Wall : IBlock, IInteractable
    {
        private Direction buttonDirs;
        private bool isClockWise;

        public Wall(Vector2Int coord, Direction buttonDirs = Direction.None, bool isClockWise = true) {
            Coord = coord;
            this.buttonDirs = buttonDirs;
            this.isClockWise = isClockWise;
        }

        public Vector2Int Coord { get; private set; }

        public event Action OnInteracted;

        void IInteractable.Interact(ILocatable mob, Direction dir)
        {
            if (buttonDirs.hasCommonFlags(dir))
                Coord.TryRotate(isClockWise);
        }
    }
}