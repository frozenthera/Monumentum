using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum.Model
{
    public static partial class BlockFactory
    {
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Directions dir, int durablity) => CreateBlock(type, coord, dir, null, durablity);
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, IStage nextStage, Vector2Int nextCoord) => CreateBlock(type, coord, Directions.None, nextStage, -1, nextCoord);
        public static IBlock CreateBlock(this BlockType type, Vector2Int coord, Directions dir = Directions.None, IStage nextStage = null, int durablity = -1, Vector2Int nextCoord = new Vector2Int())
        {
            IBlock newBlock = null;
            switch (type)
            {
                case BlockType.Normal:
                    newBlock = new NormalTile(coord, dir, durablity);
                    break;
                case BlockType.Wall:
                    newBlock = new Wall(coord, dir);
                    break;
                case BlockType.Portal:
                    newBlock = new Portal(coord, nextStage, nextCoord);
                    break;
                case BlockType.RotateTile:
                    newBlock = new RotatableTile(coord, dir, durablity);
                    break;
            }

            coord.AddBlock(newBlock);
            OnBaseCreated?.Invoke(type, newBlock);
            return newBlock;
        }

        public static void CreateItem(this RelicEffect effect, Vector2Int coord)
        {
            //new Relic(effect.GainRelic());
        }

        public static void CreateRoad(Vector2Int coord, Directions directions)
        {
            if(coord.HasBlock(out IRoadLayable r))
            {
                r.OpenDirections = directions;
                foreach (var d in directions.ToSoleDirs())
                    OnRoadCreated.Invoke(coord, d);
            }
        }

        public static void SetDurability(Vector2Int coord, int durability)
        {
            if (coord.HasBlock(out IHavingDurability dur))
            {
                dur.Durability = durability;
            }
        }

        public static List<IWallHanging> CreateHanging(this HangingType type, Vector2Int coord, Directions dirs = (Directions)15)
        {
            if (!coord.HasBlock(out IWall wall))
                throw new NullReferenceException();

            var output = new List<IWallHanging>();
            foreach (SoleDir dir in dirs.ToSoleDirs())
            {
                var newHanging = GetInstance(wall, dir);
                wall.AddHanging(newHanging);
                output.Add(newHanging);
            }

            return output;

            IWallHanging GetInstance(IWall hungWall, SoleDir dir)
            {
                switch (type)
                {
                    case HangingType.Button:
                        return new Button(hungWall, dir);
                    case HangingType.Power:
                        return new PowerGenerator(hungWall, dir);
                    case HangingType.Receiver:
                        return new PowerReciver(hungWall, dir);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public static event Action<BlockType, IBlock> OnBaseCreated;
        public static event Action<Vector2Int, SoleDir> OnRoadCreated;

        public static event Action<IStage, Vector2Int> OnPortalUsed;
        public static bool IsPortalUnlocked { private get; set; } = false;
    }
}