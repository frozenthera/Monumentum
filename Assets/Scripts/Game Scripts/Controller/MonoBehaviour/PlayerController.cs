using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monumentum.Model;

namespace Monumentum.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private const float Speed = 2f;

        private Player player;

        public void OnStartGame()
        {
            player = Player.Create(Vector2Int.zero);
            player.OnMoved += (o => transform.position = o.ToVector3());
            player.OnRotated += (o => StartCoroutine(Rotate90Character(o, 0.3f)));
        }

        public void Warp(Vector2Int coord)
        {
            player.Warp(coord);
        }

        public void Walk(Vector2 keyInput)
        {
            player.Walk(keyInput * Speed * Time.deltaTime);
        }

        public void Interact()
        {
            player.TryInteract();
        }

        private IEnumerator Update
        {
            get
            {
                while (true)
                {
                    player.Walk(Speed * Time.deltaTime * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                    yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
                }
            }
        }

        private IEnumerator WaitForInteract
        {
            get
            {
                while (true)
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
                    player.TryInteract();
                    yield return null;
                }
            }
        }

        private IEnumerator Rotate90Character(bool isClockwise = true, float duration = 1.0f)
        {
            Vector3 center = transform.position.GetTileCenter();
            
            float radius = Vector3.Distance(transform.position, center);
            float radFrom = Mathf.Acos(-(transform.position.x - center.x) / radius) + Mathf.PI;
            float radTo = radFrom + Mathf.PI / (!isClockwise ? 2f : -2f);

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                radFrom = Mathf.Lerp(radFrom, radTo, elapsed / duration); //Quaternion.Slerp(from, to, elapsed / duration);
                transform.position = new Vector3(center.x + radius * Mathf.Cos(radFrom), center.y + radius * Mathf.Sin(radFrom));
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = new Vector3(center.x + radius * Mathf.Cos(radTo), center.y + radius * Mathf.Sin(radTo));
        }
    }
}