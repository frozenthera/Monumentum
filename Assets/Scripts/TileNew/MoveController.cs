using UnityEngine;
using Monument.Model;

namespace Monument.Controller
{
    public delegate void PositionUpdate(Vector3 position);
    public class MoveController
    {
        //private 
        private IMovable movable;
        private PositionUpdate PositionUpdate;
        public bool CanMove { get; private set; } = true;

        public MoveController(IMovable movable, PositionUpdate PositionUpdate)
        {
            this.movable = movable;
            this.PositionUpdate = PositionUpdate;
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

        public void Move(Vector2 vector2)
        {
            movable.Move(vector2);
        }

        private void UpdatePosition()
        {
            PositionUpdate(movable.Position.ToVector3());
        }
    }
}