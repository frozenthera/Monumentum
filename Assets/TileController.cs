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

        IEnumerator OnMouseExit()
        {
            yield return new WaitUntil(() => !Input.GetButton("Fire1"));
            outline.enabled = false;
        }

        void OnMouseOver()
        {
            if (PlayerInput.inst.Pull)
                outline.enabled = true;
        }
    }
}