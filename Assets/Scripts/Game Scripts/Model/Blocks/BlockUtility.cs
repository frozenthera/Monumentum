using System.Collections.Generic;
using UnityEngine;

namespace Monument.Model
{
    public static class BlockUtility
    {
        public static bool TryMoveBlock(this IMovableBlock block, Direction direction)
        {
            Vector2Int destination = block.Coord + direction.ToVector2Int();

            if (destination.IsEmpty() && Player.Singleton.Position.ToVector2Int() != block.Coord)
            {
                block.MoveBlock(block.Coord, destination);
                block.Coord = destination;
                return true;
            }
            return false;
        }

        public static bool CanStandOn(this Vector2 position)
        {
            Vector2Int coord = position.ToVector2Int();
            
            if(coord.HasBlock(out ITile t))
            {
                Direction openDirections = t.OpenDirections;

                if (openDirections.HasFlag(Direction.Up))
                    if (Direction.Up.GetAccessibleSpace(coord).Contains(position))
                        return true;
                if (openDirections.HasFlag(Direction.Down))
                    if (Direction.Down.GetAccessibleSpace(coord).Contains(position))
                        return true;
                if (openDirections.HasFlag(Direction.Left))
                    if (Direction.Left.GetAccessibleSpace(coord).Contains(position))
                        return true;
                if (openDirections.HasFlag(Direction.Right))
                    if (Direction.Right.GetAccessibleSpace(coord).Contains(position))
                        return true;
            }
            return false;
        }
        
        public static void TryInteract(this ILocatable mob)
        {
            if (TryGetExpectedCoord(mob.Position, out Vector2Int coord, out Direction dir))
                if (coord.HasBlock(out IInteractable pushable))
                    pushable.Interact(mob, dir);

            bool TryGetExpectedCoord(Vector2 position, out Vector2Int expectedCoord, out Direction pushDir)
            {
                Vector2 distFromCenter = position - position.ToVector2Int();
                expectedCoord = position.ToVector2Int();
                pushDir = Direction.None;

                switch (distFromCenter)
                {
                    case var d when (GetRect(Direction.Up).Contains(d)):
                        expectedCoord += Vector2Int.up;
                        pushDir = Direction.Up;
                        return true;
                    case var d when (GetRect(Direction.Down).Contains(d)):
                        expectedCoord += Vector2Int.down;
                        pushDir = Direction.Down;
                        return true;
                    case var d when (GetRect(Direction.Left).Contains(d)):
                        expectedCoord += Vector2Int.left;
                        pushDir = Direction.Left;
                        return true;
                    case var d when (GetRect(Direction.Right).Contains(d)):
                        expectedCoord += Vector2Int.right;
                        pushDir = Direction.Right;
                        return true;
                }
                
                return false;

                Rect GetRect(Direction direction)
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

        //반드시 낱개의 direction에만 적용하시오.
        private static Rect GetAccessibleSpace(this Direction direction, Vector2Int coord)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathWidth / 2f, PathWidth, PathHeight);
                case Direction.Down:
                    return new Rect(coord.x - PathWidth / 2f, coord.y - PathHeight, PathWidth, PathHeight);
                case Direction.Left:
                    return new Rect(coord.x - 1 / 2f, coord.y - PathWidth / 2f, PathHeight, PathWidth);
                case Direction.Right:
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