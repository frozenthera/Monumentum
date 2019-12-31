using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Monument.Model;

namespace Monument
{
    public static class EnumUtility
    {
        public static int GetFlagCount(this Direction flag)
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

        public static Direction Rotate(this Direction dir, bool isClockwise = true)
        {
            int dirCode = (int)dir;

            if (isClockwise)
            {
                dirCode <<= 1;
                dirCode = dirCode % 16 + dirCode / 16;
            }
            else
            {
                int rest = dirCode % 2;
                dirCode >>= 1;
                dirCode += rest * 8;
            }

            return (Direction)dirCode;
        }

        public static bool hasCommonFlags(this Direction dir1, Direction dir2)
        {
            return (dir1.ToInt() & dir2.ToInt()) != 0;
        }

        public static int ToInt(this Direction dir)
        {
            return (int)dir == -1 ? 15 : (int)dir;
        }
    }
}