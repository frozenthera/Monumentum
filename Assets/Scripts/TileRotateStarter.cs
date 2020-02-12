using Monumentum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum {
    public class TileRotateStarter : MonoBehaviour
    {
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RotateNearTiles()
        {
            transform.position.GetNearPositions().ForEach(
                p => {
                    if(p.HasBlock(out ChainRotator r))
                    {
                        Debug.Log(3);
                        r.Rotate();
                    }
                }
            );
        }
    }
}