using UnityEngine;

namespace Monumentum.Model
{
    [CreateAssetMenu(menuName = nameof(RelicData))]
    public class RelicData : ScriptableObject
    {
        [SerializeField]
        private RelicEffect effect;
    }
}