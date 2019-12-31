using Monument.Controller;
using Monument.Skin;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monument.Model.Serialized
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

        public void ChangeTo()
        {
            MapObjectFactory.ResetMap();
            Distributions.ForEach(d => d.ApplyToMap(theme.LoadTheme));
        }

        private delegate void MapCallback(BlockType type, IBlock info);
    }
}