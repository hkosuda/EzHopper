using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class DemoUtils
{
    static public float[] Interpolate(List<float[]> dataList, float pastTime)
    {
        if (dataList == null || dataList.Count == 0) { return new float[2] { 0.0f, 0.0f }; }

        var indexes = GetIndexes(dataList, pastTime);
        if (indexes[0] == indexes[1]) { return dataList[indexes[0]]; }

        var rate = GetRate(indexes, dataList, pastTime);
        var interpolatedData = new float[PlayerRecorder.dataSize];

        for (var n = 0; n < PlayerRecorder.dataSize; n++)
        {
            if (n == 4 || n == 5)
            {
                var a0 = dataList[indexes[0]][n];
                var a1 = dataList[indexes[1]][n];

                interpolatedData[n] = AngleInterPolation(a0, a1, rate[0], rate[1]);
            }

            else
            {
                interpolatedData[n] = dataList[indexes[0]][n] * rate[0] + dataList[indexes[1]][n] * rate[1];
            }
        }

        return interpolatedData;

        // - inner function
        static int[] GetIndexes(List<float[]> dataList, float pastTime)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new int[2] { 0, 0 };
            }

            for (var n = 0; n < dataList.Count; n++)
            {
                var data = dataList[n];
                if (data[0] < pastTime) { continue; }
                if (n == 0) { return new int[2] { 0, 0 }; }
                return new int[2] { n - 1, n };
            }

            var lastIndex = dataList.Count - 1;
            return new int[2] { lastIndex, lastIndex };
        }

        // - inner function
        static float[] GetRate(int[] indexes, List<float[]> dataList, float pastTime)
        {
            if (indexes[0] == indexes[1]) { return new float[2] { 0.5f, 0.5f }; }

            var t0 = dataList[indexes[0]][0];
            var t1 = dataList[indexes[1]][0];

            var dt = t1 - t0;

            var rate0 = Calcf.SafetyDiv(t1 - pastTime, dt, 0.0f);
            var rate1 = 1.0f - rate0;

            return new float[2] { rate0, rate1 };
        }

        // - inner function
        static float AngleInterPolation(float a0, float a1, float r0, float r1)
        {
            if (a0 < 100.0f && a1 > 300.0f)
            {
                return (a0 + 360.0f) * r0 + a1 * r1;
            }

            if (a0 > 300.0f && a1 < 100.0f)
            {
                return a0 * r0 + (a1 + 360.0f) * r1;
            }

            return a0 * r0 + a1 * r1;
        }
    }
}
