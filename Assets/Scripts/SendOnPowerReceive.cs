using Gamekit3D.GameCommands;
using Monumentum.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public class SendOnPowerReceive : TriggerCommand, IPowerReactable
    {
        public Vector3 Position => transform.position;

        public Directions ForcePower(SoleDir dir)
        {
            Send();
            return Directions.None;
        }
    }
}