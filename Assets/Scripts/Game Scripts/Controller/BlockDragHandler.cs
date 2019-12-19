using Monument.Model;
using System.Collections;
using UnityEngine;

namespace Monument.Controller
{
    public class BlockDragHandler : MonoBehaviour
    {
        private MoveIntController controller;
        public void Load(IMovableBlock movable)
        {
            controller = new MoveIntController(movable, UpdatePosition);
            //tile.OpenDirections;
        }

        private void OnMouseDown()
        {
            StartCoroutine(DragStart);

            //controller.Move(Direction.Right);
        }

        private void UpdatePosition(Vector2Int coord)
        {
            Vector3 destination = coord.ToVector3();
            transform.position = destination;
        }

        IEnumerator DragStart
        {
            get
            {
                
                Vector3 start = Input.mousePosition;
                yield return new WaitUntil(() => !Input.GetMouseButton(0));

                Vector3 delta = Input.mousePosition - start;
                const int DragMinJudge = 100;

                if (delta.magnitude > DragMinJudge)
                {
                    Vector3 deltaDir = delta.normalized;
                    Direction dir = Direction.None;
                    if (Mathf.Abs(deltaDir.x) > Mathf.Abs(deltaDir.y))
                    {
                        if (deltaDir.x > 0)
                            dir = Direction.Right;
                        else
                            dir = Direction.Left;
                    }
                    else
                    {
                        if (deltaDir.y > 0)
                            dir = Direction.Up;
                        else
                            dir = Direction.Down;
                    }

                    controller.Move(dir);
                }//vector3 to dir
            }
        }
    }

    
}