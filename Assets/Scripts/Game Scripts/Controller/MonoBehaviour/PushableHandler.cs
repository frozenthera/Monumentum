using System.Collections;
using System.Collections.Generic;
using Monument.Model;
using UnityEngine;

namespace Monument.Controller
{
    public class PushableHandler : MonoBehaviour
    {
        private IPushable pushable;

        public void Load(IPushable pushable)
        {
            this.pushable = pushable;
            StartCoroutine(WaitForPush);
        }

        private IEnumerator WaitForPush {
            get
            {
                while (true)
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
                    pushable.Push(Player.Singleton);
                    yield return null;
                }
            }
        }

    }
}