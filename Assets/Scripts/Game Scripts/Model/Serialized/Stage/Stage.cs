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
        private TileDistribution[] tiles;
        [SerializeField]
        private PortalDistribution[] portals;

        private IEnumerable<IDistribution> Distributions => blocks.Union<IDistribution>(tiles).Union(portals);

        void IStage.ChangeTo()
        {
            Theme.SetCurrentTheme(theme);
            MapUtility.ResetMap();
            Distributions.ForEach(d => d.ApplyToMap());
        }
    }
}


