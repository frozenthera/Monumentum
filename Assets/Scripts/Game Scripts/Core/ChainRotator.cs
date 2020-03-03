using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public class ChainRotator : MonoBehaviour
    {
        bool isDuringRotate = false;

        public void StartRotate(bool isClockwise = true)
        {
            if (isDuringRotate)
                return;

            isDuringRotate = true;
            PowerSource.OnBlockUpdated();

            Queue<ChainRotator> searchingRotators = new Queue<ChainRotator>();
            HashSet<ChainRotator> nextRotators = new HashSet<ChainRotator>();
            HashSet<ChainRotator> searchEnded = new HashSet<ChainRotator>();

            searchingRotators.Enqueue(this);

            do SearchCurPhase();
            while (searchingRotators.Count > 0);

            void SearchCurPhase()
            {
                while (searchingRotators.Count > 0)
                {
                    var cur = searchingRotators.Dequeue();
                    cur.RotateBlock(isClockwise);
                    searchEnded.Add(cur);
                    cur.transform.position.GetNearPositions().ForEach(c => { 
                        if (c.HasBlock(out ChainRotator cr) && !searchEnded.Contains(cr)) 
                            nextRotators.Add(cr);
                    });
                }

                nextRotators.ForEach(c => searchingRotators.Enqueue(c));
                nextRotators.Clear();
                isClockwise = !isClockwise;
            }
        }

        void RotateBlock(bool isClockwise = true)
        {
            GetComponent<PowerWire>()?.OnRotate(isClockwise);
            StartCoroutine(Rotate(Vector3.up, isClockwise ? -90 : 90));

            IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
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

                isDuringRotate = false;
            }
        }
    }
}