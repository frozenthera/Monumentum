using System;

namespace Monumentum.Model
{
    public interface IInteractable
    {
        void Interact(ILocatable mob, Directions dir = Directions.None);
        event Action<Directions> OnInteracted;
    }
}