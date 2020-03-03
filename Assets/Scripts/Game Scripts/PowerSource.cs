using Monumentum.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public class PowerSource : MonoBehaviour
    {
        static readonly HashSet<PowerSource> powerSources = new HashSet<PowerSource>();

        public Directions directions;

        public static void OnBlockUpdated()
        {
            foreach (var p in powerSources)
            {
                p.TriggerPower();
            }
        }

        [ContextMenu(nameof(TriggerPower))]
        void TriggerPower()
        {
            Queue<(IPowerReactable, SoleDir)> searchingReactables = new Queue<(IPowerReactable, SoleDir)>();
            foreach (var dir in directions.ToSoleDirs())
            {
                var targetPos = transform.position + dir.ToVector3();
                if (targetPos.HasBlock(out IPowerReactable reactor))
                {
                    searchingReactables.Enqueue((reactor, dir.ToReverse()));
                }
            }

            HashSet<(IPowerReactable, SoleDir)> nextReactables = new HashSet<(IPowerReactable, SoleDir)>();
            HashSet<(IPowerReactable, SoleDir)> searchEnded = new HashSet<(IPowerReactable, SoleDir)>();

            while (searchingReactables.Count > 0)
            {
                while (searchingReactables.Count > 0)
                {
                    (IPowerReactable reactable, SoleDir curDir) = searchingReactables.Dequeue();
                    var nextDirs = reactable.ForcePower(curDir);
                    foreach (var dir in nextDirs.ToSoleDirs())
                    {
                        var targetPos = reactable.Position + dir.ToVector3();
                        if (targetPos.HasBlock(out IPowerReactable reactor) && !searchEnded.Contains((reactor, dir.ToReverse())))
                        {
                            //nextReactables.Add((reactor, dir.ToReverse()));
                        }
                    }
                }
                nextReactables.ForEach(tuple => searchingReactables.Enqueue(tuple));
                nextReactables.Clear();
            }
        }

        /*public static void TriggerPower(this Vector2Int coord, SoleDir dir)
        {
            Queue<(Vector2Int, SoleDir)> searchingCoords = new Queue<(Vector2Int, SoleDir)>();
            searchingCoords.Enqueue((coord + dir.ToVector2Int(), dir.ToReverse()));
            HashSet<(Vector2Int, SoleDir)> nextCoords = new HashSet<(Vector2Int, SoleDir)>();
            HashSet<(Vector2Int, SoleDir)> searchEnded = new HashSet<(Vector2Int, SoleDir)>();

            do Search();
            while (searchingCoords.Count > 0);

            void Search()
            {
                while (searchingCoords.Count > 0)
                {
                    (Vector2Int curCoord, SoleDir curDir) = searchingCoords.Dequeue();
                    if (curCoord.HasBlock(out IPowerReactable p) && !searchEnded.Contains((curCoord, curDir)))
                    {
                        searchEnded.Add((curCoord, curDir));
                        curCoord
                            .GetNearCoordAndItsDir(p.ForcePower(curDir))
                            .ForEach(o => { if (!searchEnded.Contains(o)) { nextCoords.Add(o); } });
                    }
                }

                nextCoords.ForEach(c => searchingCoords.Enqueue(c));
                nextCoords.Clear();
            }
        }*/

        private void OnEnable()
        {
            powerSources.Add(this);
        }

        private void OnDisable()
        {
            powerSources.Remove(this);
        }
    }
}