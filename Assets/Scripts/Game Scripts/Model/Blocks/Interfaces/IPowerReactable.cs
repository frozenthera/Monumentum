using System;

namespace Monumentum.Model
{
    public interface IPowerReactable
    {
        Directions ForcePower(SoleDir dir, bool turnOn = true);
        event Action<SoleDir> OnPowerChanged;
    }
}