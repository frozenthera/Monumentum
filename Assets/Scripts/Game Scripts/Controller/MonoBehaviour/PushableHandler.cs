using System.Collections;
using System.Collections.Generic;
using Monument.Model;
using UnityEngine;

namespace Monument.Controller
{
    public class PushableHandler : MonoBehaviour
    {
        private IInteractable pushable;

        public void Load(IInteractable pushable)
        {
            this.pushable = pushable;
            //StartCoroutine(WaitForPush);
        }

        private IEnumerator WaitForPush {
            get
            {
                while (true)
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
                    Debug.Log(Player.Singleton.Position);
                    pushable.Interact(Player.Singleton);
                    yield return null;
                }
            }
        }

    }
}