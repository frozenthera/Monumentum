using UnityEngine;
using Monumentum.Model;

namespace Monumentum.Controller
{
    public delegate void PositionUpdate(Vector3 position);
    public class MoveController
    {
        /*//private 
        private IWalkableMob movable;
        private PositionUpdate PositionUpdate;
        public bool CanMove { get; private set; } = true;

        public MoveController(IWalkableMob movable, PositionUpdate PositionUpdate)
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

        public void Move(Vector2 delta)
        {
            movable.Walk(delta);
        }

        private void UpdatePosition()
        {
            PositionUpdate(movable.Position.ToVector3());
        }*/
    }
}