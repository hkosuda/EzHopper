using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utils
{
    static public string TimeText(float time, bool mSec = true)
    {
        var min = Mathf.FloorToInt((int)time / 60.0f);
        time -= 60 * min;

        var sec = (int)time;
        time -= sec;

        if (mSec)
        {
            var msec = (int)(time * 100.0f);
            return PaddingZero(min) + ":" + PaddingZero(sec) + ":" + PaddingZero(msec);
        }

        else
        {
            return PaddingZero(min) + ":" + PaddingZero(sec);
        }

        // function
        static string PaddingZero(int n)
        {
            if (n < 10)
            {
                return "0" + n.ToString();
            }

            else
            {
                return n.ToString();
            }
        }
    }
}
