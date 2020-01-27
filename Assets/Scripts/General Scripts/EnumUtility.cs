using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Monumentum.Model;

namespace Monumentum
{
    public static class EnumUtility
    {
        public static int GetFlagCount(this Directions flag)
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

        public static Directions Rotate(this Directions dir, bool isClockwise = true)
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

            return (Directions)dirCode;
        }

        public static Directions ToMultiDir(this SoleDir soleDir)
        {
            return (Directions)soleDir;
        }

        public static bool HasCommonFlags(this Directions dir1, Directions dir2)
        {
            return (dir1.ToInt() & dir2.ToInt()) != 0;
        }

        public static IEnumerable<SoleDir> ToSoleDirs(this Directions input)
        {
            int digit = input.ToInt();
            for(int i = 0; i < 4; i++)
                if((digit & (1 << i)) != 0)
                    yield return (SoleDir)(1 << i);
            yield break;
        }

        public static bool TrySubtract(this Directions dir1, SoleDir dir2, out Directions output)
        {
            output = Directions.None;
            if(dir1.HasFlag((Directions)dir2))
            {
                output = (Directions)((int)dir1 - (int)dir2);
                return true;
            }
            return false;
        }

        public static int ToInt(this Directions dir)
        {
            return (int)dir == -1 ? 15 : (int)dir;
        }
    }
}