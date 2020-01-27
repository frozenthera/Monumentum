using System;

namespace Monumentum.Model
{

    public static partial class BlockFactory
    {
        private class Button : IWallHanging, IInteractable
        {
            public SoleDir Direction { get; private set; }
            private IBlock hungBlock;
            private readonly bool isClockwise = true;

            public event Action<Directions> OnInteracted;

            public void Interact(ILocatable mob, Directions dir = Directions.None)
            {
                hungBlock.Coord.TryRotate(isClockwise);
            }

            public Button(IBlock hungBlock, SoleDir dir, bool isClockwise = true)
            {
                this.hungBlock = hungBlock;
                Direction = dir;
                this.isClockwise = isClockwise;
            }
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