using System.Collections;
using Monument.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Monument.Controller
{
    public class RotatableBlockHandler : MonoBehaviour
    {
        private IRotatable rotatable;
        public void Load(IRotatable rotatable)
        {
            this.rotatable = rotatable;
            rotatable.OnRotated += () => StartCoroutine(RotateGameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                rotatable.RotateBlock();
            }
        }

        IEnumerator RotateGameObject
        {
            get
            {
                transform.Rotate(0, 0, -90);
                yield return null;
            }
        }
    }
}