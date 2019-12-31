using Monument.Model;

namespace Monument.Skin
{
    public partial class Theme
    {
        private interface IThemeSet
        {
            BlockType BlockType { get; }
            void LoadSet(IBlock block);
        }
    }
}