using Monumentum.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Controller
{
    public static class HandlerFacade
    {
        public static void AddFitComponents(this GameObject go, IBlock block)
        {
            if (block is IMovableBlock m)
                go.AddComponent<BlockDragHandler>().Load(m);
            if (block is IRotatable r)
                go.AddComponent<RotatableBlockHandler>().Load(r);
            if (block is IBreakableBlock b)
                go.AddComponent<BreakableHandler>().Load(b);
        }
    }
}