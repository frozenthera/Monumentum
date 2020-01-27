using Monumentum.Skin;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monumentum.Model.Serialized
{
    [CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(Stage))]
    public partial class Stage : ScriptableObject, IStage
    {
        [SerializeField]
        private Theme theme;

        [SerializeField]
        private BlockDistribution[] blocks;
        [SerializeField]
        private RoadDistribution[] roads;
        [SerializeField]
        private DurabilityDistribution[] durs;
        [SerializeField]
        private PortalDistribution[] portals;
        [SerializeField]
        private HangingDistribution[] hangings;

        private IEnumerable<IDistribution> Distributions => blocks.Union<IDistribution>(roads).Union(durs).Union(portals).Union(hangings);

        void IStage.Generate()
        {
            Theme.SetCurrentTheme(theme);
            MapUtility.ResetMap();
            Distributions.ForEach(d => d.ApplyToMap());
        }

        [System.Serializable]
        private class RelicDistribution : IDistribution
        {
            [SerializeField]
            private RelicData relic;
            [SerializeField]
            private Vector2Int coord;

            void IDistribution.ApplyToMap()
            {
                
            }
        }
    }
}


