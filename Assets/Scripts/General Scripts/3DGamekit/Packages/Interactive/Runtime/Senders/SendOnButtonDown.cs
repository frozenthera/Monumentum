using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit3D.GameCommands
{
    public class SendOnButtonDown : SendGameCommand
    {
        public string buttonName = "X";
        public LayerMask layers;

        bool canExecuteButtons = false;

        void OnCollisionEnter(Collision collision)
        {
            canExecuteButtons = true;
                
        }

        void OnCollisionExit(Collision collision)
        {
            canExecuteButtons = false;
        }

        private void Update()
        {
            if(canExecuteButtons && Input.GetButtonDown(buttonName))
                Send();
        }
    }
}