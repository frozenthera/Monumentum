using Monument.Model;
using UnityEngine;

namespace Monument.Controller
{
    public class BlockController : MonoBehaviour
    {
        private ITile tile;
        [SerializeField]
        private BlockDragHandler blockDragHandler;
        public void Load(IBlock block)
        {
            blockDragHandler?.Load(block as IMovableBlock);
            //tile.OpenDirections;
        }
    }
}