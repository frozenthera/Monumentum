using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private class Wall : IBlock, IWall, IInteractable, IPowerReactable, IRearrangingTarget
        {
            private readonly List<IWallHanging> hangings = new List<IWallHanging>();

            public Wall(Vector2Int coord, Directions buttonDirs = Directions.None, bool isClockwise = true)
            {
                Coord = coord;
                foreach (SoleDir dir in buttonDirs.ToSoleDirs())
                {
                    hangings.Add(new Button(this, dir, isClockwise));
                }
            }

            public Vector2Int Coord { get; private set; }

            Directions IPowerReactable.ForcePower(SoleDir dir, bool turnOn)
            {
                foreach (var h in hangings)
                    if (h is IPowerReactable i)
                        i.ForcePower(dir, turnOn);
                OnPowerChanged?.Invoke(dir);
                return Directions.None;
            }
            
            public event Action<SoleDir> OnPowerChanged;

            void IWall.AddHanging(IWallHanging hanging)
            {
                hangings.Add(hanging);
            }

            void IInteractable.Interact(ILocatable mob, Directions dir)
            {
                foreach (var h in hangings)
                    if (h is IInteractable i)
                        i.Interact(mob, dir);
                OnInteracted?.Invoke(dir);
            }

            void IRearrangingTarget.UpdateByRearrange()
            {
                foreach (var h in hangings)
                    if (h is IRearrangingTarget r)
                        r.UpdateByRearrange();
            }

            public event Action<Directions> OnInteracted;
        }
    }
}

/*private class WallHangingInfo
        {
            private Vector2Int coord;

            public Directions Dir { get; set; }
            /*public Action Interact { get; private set; }

            public Action Update { get; private set; }

            public Action AddPower { get; private set; }

            public static WallHangingInfo Create(Wall wall, HangingType type, Vector2Int coord, Directions directions, bool isClockwise = true)
            {
                var newInfo = new WallHangingInfo();
                newInfo.coord = coord;
                newInfo.Dir = directions;
                
                switch (type)
                {
                    case HangingType.Button:
                        wall.OnInteracted += (dir) =>
                        {
                            if (newInfo.Dir.HasCommonFlags(dir))
                                newInfo.coord.TryRotate(isClockwise);
                        };
                        break;
                    case HangingType.Power:
                        //BlockUtility.OnBlockRearranged += () => coord.TriggerPower(newInfo.Dir);
                        break;
                    case HangingType.Receiver:
                        wall.OnPowerChanged += (b) => Debug.Log("activated");
                        break;
                }
                
                return newInfo;
            }
        }*/
