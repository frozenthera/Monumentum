using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Controller.Main
{
    public class KeyManager : MonoBehaviour
    {
        public void Init()
        {
            StartCoroutine(Reset);
            StartCoroutine(WaitForInteract);
            StartCoroutine(Walk);
        }

        private IEnumerator Walk
        {
            get
            {
                while (true)
                {
                    yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
                    OnTryWalk?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
                    yield return null;
                }
            }
        }
        public event Action<Vector2> OnTryWalk;

        private IEnumerator WaitForInteract
        {
            get
            {
                while (true)
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
                    OnTryInteract?.Invoke();
                    yield return null;
                }
            }
        }
        public event Action OnTryInteract;

        private IEnumerator Reset
        {
            get
            {
                while(true)
                {
                    if(Input.GetKeyDown(KeyCode.R))
                        GameUtility.DoReset();
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
                }
            }
        }
    }
}