using System.Collections;
using System.Collections.Generic;
using Monumentum.Model;
using UnityEngine;

namespace Monumentum.Controller
{
    public class BreakableHandler : MonoBehaviour
    {
        private IBreakableBlock breakable;

        public void Load(IBreakableBlock breakable)
        {
            this.breakable = breakable;
            breakable.OnBreaking += BeBroken;
        }

        private void BeBroken()
        {
            Destroy(gameObject);
        }
    }
}