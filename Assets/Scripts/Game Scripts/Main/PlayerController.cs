using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monument.Model;

namespace Monument.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private const float Speed = 2f;

        private readonly Player player = Vector2Int.zero.CreatePlayer();
        private MoveController move;

        public void Init()
        {
            move = new MoveController(player, (position) => transform.position = position);
            StartCoroutine(Update);
        }

        private IEnumerator Update
        {
            get
            {
                while (true)
                {
                    move.Move(Speed * Time.deltaTime * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                    yield return null;
                    yield return new WaitUntil(() => move.CanMove);
                }
            }
        }
    }
}