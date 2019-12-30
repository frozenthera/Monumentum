using System;

namespace Monument.Model
{
    public interface IInteractable : IBlock
    {
        void Interact(ILocatable mob);
        event Action OnInteracted;
    }
}