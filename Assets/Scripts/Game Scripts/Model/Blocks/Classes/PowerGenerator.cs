namespace Monumentum.Model
{

    public static partial class BlockFactory
    {
        private class PowerGenerator : IWallHanging, IRearrangingTarget
        {
            public SoleDir Direction { get; private set;  }
            private IBlock hungBlock;

            public void UpdateByRearrange()
            {
                hungBlock.Coord.TriggerPower(Direction);
            }

            public PowerGenerator(IBlock hungBlock, SoleDir dir)
            {
                this.hungBlock = hungBlock;
                Direction = dir;
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
