using UnityEngine;

namespace Monument.Model
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField]
        private BlockType tileType;
        private ITile tile;

        public void Load(IBlock tile)
        {
            //tile.OpenDirections;
        }
    }
}