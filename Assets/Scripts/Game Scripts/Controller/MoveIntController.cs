using Monument.Model;
using UnityEngine;

namespace Monument.Controller
{
    public delegate void CoordUpdate(Vector2Int coord);
    public class MoveIntController
    {
        //private 
        private IMovableBlock movable;
        private CoordUpdate CoordUpdate;
        public bool CanMove { get; private set; } = true;

        public MoveIntController(IMovableBlock movable, CoordUpdate CoordUpdate)
        {
            this.movable = movable;
            this.CoordUpdate = CoordUpdate;
            movable.OnMoved += UpdatePosition;
        }

        public void ProhibitMove()
        {
            CanMove = false;
        }

        public void AllowMove()
        {
            CanMove = true;
        }

        public void Move(Direction dir)
        {
            movable.MoveBlock(dir);
        }

        private void UpdatePosition()
        {
            CoordUpdate(movable.Coord);
        }
    }
}