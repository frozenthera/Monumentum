using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        private partial class NormalTile : IBreakableBlock, IMovableBlock, ITile, IPowerReactable,
            IRoadLayable, IHavingDurability
        {
            private readonly List<ITileComponent> components = new List<ITileComponent>();

            public NormalTile(Vector2Int coord)
            {
                this.Coord = coord;
            }

            public NormalTile(Vector2Int coord, Directions openDirections, int durablity = -1)
            {
                this.Coord = coord;
            }


            public Vector2Int Coord { get; set; }

            public Directions OpenDirections
            {
                get
                {
                    foreach (var c in components)
                        if (c is RoadComponent r)
                            return r.Directions;
                    return Directions.None;
                }
            }

            #region 인터페이스 구현
            Directions IRoadLayable.OpenDirections
            {
                set => components.Add(new RoadComponent(value));
            }

            int IHavingDurability.Durability
            {
                set => components.Add(new BreakableComponent(this, value));
            }

            void IMovableBlock.MoveBlock(Directions direction)
            {
                if (this.TryMoveBlock(direction))
                    OnMoved?.Invoke();
            }

            void IBreakableBlock.DamageBlock()
            {
                components.ForEachWithCasting<ITileComponent, IBreakableBlock>(b => b.DamageBlock());
            }

            /// <summary>
            /// 전기를 가합니다.
            /// </summary>
            /// <param name="dir">전기를 작용하는 방향입니다.</param>
            /// <returns>밖으로 나가는 전기의 방향을 반환합니다.</returns>
            Directions IPowerReactable.ForcePower(SoleDir dir, bool turnOn)
            {
                bool isTurnedOn = OpenDirections.TrySubtract(dir, out Directions output);
                OnPowerChanged?.Invoke(dir);
                return output;
            }
            #endregion

            public event Action<SoleDir> OnPowerChanged;

            public event Action OnMoved;
            public event Action OnBreaking;

            private interface ITileComponent
            {

            }

            private class RotatableComponent : ITileComponent
            {

            }
        }
    }
}