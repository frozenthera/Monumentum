using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static class BlockUtility
    {
        public static bool TryMoveBlock(this IMovableBlock block, Directions direction)
        {
            Vector2Int destination = block.Coord + direction.ToVector2Int();

            if (destination.IsEmpty() && !block.Coord.LocatesPlayer())
            {
                block.MoveBlock(block.Coord, destination);
                block.Coord = destination;
                MapUtility.InformRearranging();
                return true;
            }
            return false;
        }
        
        public static void TryInteract(this ILocatable mob)
        {
            if (TryGetExpectedCoord(mob.Position, out Vector2Int coord, out Directions dir))
                if (coord.HasBlock(out IInteractable i))
                    i.Interact(mob, dir);

            bool TryGetExpectedCoord(Vector2 position, out Vector2Int expectedCoord, out Directions pushDir)
            {
                Vector2 distFromCenter = position - position.ToVector2Int();
                expectedCoord = position.ToVector2Int();
                pushDir = Directions.None;

                switch (distFromCenter)
                {
                    case var d when (GetRect(Directions.Up).Contains(d)):
                        expectedCoord += Vector2Int.up;
                        pushDir = Directions.Up;
                        return true;
                    case var d when (GetRect(Directions.Down).Contains(d)):
                        expectedCoord += Vector2Int.down;
                        pushDir = Directions.Down;
                        return true;
                    case var d when (GetRect(Directions.Left).Contains(d)):
                        expectedCoord += Vector2Int.left;
                        pushDir = Directions.Left;
                        return true;
                    case var d when (GetRect(Directions.Right).Contains(d)):
                        expectedCoord += Vector2Int.right;
                        pushDir = Directions.Right;
                        return true;
                }
                
                return false;

                Rect GetRect(Directions direction)
                {
                    Rect rect = InteractRect;
                    rect.position += direction.ToVector2() * JudgeUnit * 2;
                    return rect;
                }
            }
        }

        public static void TryRotate(this Vector2Int coord, bool isClockwise = true)
        {
            Queue<Vector2Int> searchingCoords = new Queue<Vector2Int>(coord.GetNearCoords());
            HashSet<Vector2Int> nextCoords = new HashSet<Vector2Int>();
            HashSet<Vector2Int> searchEnded = new HashSet<Vector2Int>();

            do SearchCurPhaseCoords();
            while (searchingCoords.Count > 0);

            void SearchCurPhaseCoords()
            {
                while (searchingCoords.Count > 0)
                {
                    Vector2Int curCoord = searchingCoords.Dequeue();
                    if (curCoord.HasBlock(out IRotatable r) && !searchEnded.Contains(curCoord))
                    {
                        r.RotateBlock(isClockwise);
                        searchEnded.Add(curCoord);
                        curCoord.GetNearCoords().ForEach(c => { if (!searchEnded.Contains(c)) { nextCoords.Add(c); } });
                    }
                }

                nextCoords.ForEach(c => searchingCoords.Enqueue(c));
                nextCoords.Clear();
                isClockwise = !isClockwise;
            }
        }

        public static void TriggerPower(this Vector2Int coord, SoleDir dir)
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
        }

        public static void GainRelic(this RelicEffect relic)
        {
            switch (relic)
            {
                case RelicEffect.KingHall:
                    CanMoveTile = true;
                    break;
                case RelicEffect.Ether:
                    break;
            }
        }

        public static bool CanMoveTile { get; private set; } = false;
        public static bool CanPushButtons { get; private set; } = false;
        public static bool CanReset { get; private set; } = false;

        public static bool CanStandOn(this Vector2 position)
        {
            Vector2Int coord = position.ToVector2Int();

            if (coord.HasBlock(out ITile t))
            {
                Directions openDirections = t.OpenDirections;

                if (openDirections.HasFlag(Directions.Up))
                    if (Directions.Up.GetAccessibleSpace(coord).Contains(position))
                        return true;
                if (openDirections.HasFlag(Directions.Down))
                    if (Directions.Down.GetAccessibleSpace(coord).Contains(position))
                        return true;
                if (openDirections.HasFlag(Directions.Left))
                    if (Directions.Left.GetAccessibleSpace(coord).Contains(position))
                        return true;
                if (openDirections.HasFlag(Directions.Right))
                    if (Directions.Right.GetAccessibleSpace(coord).Contains(position))
                        return true;
            }
            return false;
        }

        private static Rect GetAccessibleSpace(this Directions direction, Vector2Int coord)
        {
            switch (direction)
            {
                case Directions.Up:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathWidth / 2f, PathWidth, PathHeight);
                case Directions.Down:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathHeight, PathWidth, PathHeight);
                case Directions.Left:
                    return new Rect(coord.x - 1 / 2f, coord.y - PathWidth / 2f, PathHeight, PathWidth);
                case Directions.Right:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathWidth / 2f, PathHeight, PathWidth);
                default:
                    return new Rect();
            }
        }

        private static readonly Rect InteractRect = JudgeUnit.ToSquare();

        private const float BlockUnit = 1f;
        private const float JudgeUnit = 1 / 5f;
        private const float PathWidth = JudgeUnit;
        private const float PathHeight = (PathWidth + 1) / 2f;
    }
}