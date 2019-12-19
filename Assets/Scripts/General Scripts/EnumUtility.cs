using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Monument
{
    public static class EnumUtility
    {
        public static int GetFlagCount(this Model.Direction flag)
        {
            int lValue = (int)flag;
            if (lValue == -1)
                return 4;
            int iCount = 0;
            //Loop the value while there are still bits
            while (lValue != 0)
            {
                //Remove the end bit
                lValue = lValue & (lValue - 1);

                //Increment the count
                iCount++;
            }

            //Return the count
            return iCount;
        }
    }
}