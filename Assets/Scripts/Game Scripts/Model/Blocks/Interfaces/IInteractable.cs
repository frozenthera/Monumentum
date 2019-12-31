using System;

namespace Monumentum.Model
{
    public interface IInteractable : IBlock
    {
        void Interact(ILocatable mob, Direction dir = Direction.None);
        event Action OnInteracted;
    }
}