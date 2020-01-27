using Monumentum.Model;
using System.Collections;
using UnityEngine;

namespace Monumentum.Controller
{
    [RequireComponent(typeof(BoxCollider2D))]
    internal class BlockDragHandler : MonoBehaviour
    {
        private MoveIntController controller;
        private IEnumerator moveProcess;
        public void Load(IMovableBlock movable)
        {
            controller = new MoveIntController(movable, UpdatePosition);
            //tile.OpenDirections;
        }

        private void OnMouseDown()
        {
            StartCoroutine(DragStart);
        }

        private void UpdatePosition(Vector2Int coord)
        {
            Vector3 destination = coord.ToVector3();
            //transform.position = destination;
            if (moveProcess != null)
                StopCoroutine(moveProcess);
            StartCoroutine(moveProcess = MoveSoftly(coord));
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
                    Directions dir = Directions.None;
                    if (Mathf.Abs(deltaDir.x) > Mathf.Abs(deltaDir.y))
                    {
                        if (deltaDir.x > 0)
                            dir = Directions.Right;
                        else
                            dir = Directions.Left;
                    }
                    else
                    {
                        if (deltaDir.y > 0)
                            dir = Directions.Up;
                        else
                            dir = Directions.Down;
                    }

                    controller.Move(dir);
                }
            }
        }

        IEnumerator MoveSoftly(Vector2Int coord)
        {
            const float speed = 10f;
            float count = 100;
            while (count > 0) {
                transform.position = Vector3.Lerp(transform.position, coord.ToVector3(), speed * Time.deltaTime);
                count--;
                yield return null;
            }
        }
    }
}