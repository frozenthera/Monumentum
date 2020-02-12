using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public class TileController : MonoBehaviour
    {
        Outline outline;

        void Awake()
        {
            outline = GetComponent<Outline>();
        }

        private void Update()
        {
            
        }

        public void OnPullStart()
        {
            outline.enabled = true;
        }

        IEnumerator OnMouseExit()
        {
            yield return new WaitUntil(() => !Input.GetButton("Fire1") && PlayerController.inst.CanPull);
            outline.enabled = false;
        }
    }
}