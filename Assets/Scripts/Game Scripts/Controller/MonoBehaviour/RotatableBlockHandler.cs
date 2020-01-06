using System.Collections;
using Monumentum.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Controller
{
    internal class RotatableBlockHandler : MonoBehaviour
    {
        private IRotatable rotatable;
        public void Load(IRotatable rotatable)
        {
            this.rotatable = rotatable;
            rotatable.OnRotated += Rotate90Smoothly;
        }

        private const float Duration = 0.3f;
        private void Rotate90Smoothly(bool isClockwise = true)
        {
            StartCoroutine(Rotate(Vector3.forward, isClockwise ? -90 : 90, Duration));
        }

        //https://answers.unity.com/questions/1236494/how-to-rotate-fluentlysmoothly.html
        private IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
        {
            Quaternion from = transform.rotation;
            Quaternion to = transform.rotation;
            to *= Quaternion.Euler(axis * angle);

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = to;
        }
    }
}