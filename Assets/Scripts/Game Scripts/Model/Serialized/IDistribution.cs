namespace Monument.Model.Serialized
{
    public partial class Stage
    {
        private interface IDistribution
        {
            void ApplyToMap(MapCallback callback);
        }
    }
}