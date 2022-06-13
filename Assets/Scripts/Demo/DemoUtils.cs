using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class DemoUtils
{
    static readonly int interpolationSize = 0;

    static public float[] Interpolate(List<float[]> dataList, float pastTime)
    {
        if (dataList == null || dataList.Count == 0) { return new float[2] { 0.0f, 0.0f }; }

        var indexes = GetIndexes(dataList, pastTime);
        if (indexes == null) { return new float[PlayerRecorder.dataSize]; }

        var x = GetX(dataList, indexes);
        var yList = GetYList(dataList, indexes);

        var interpolatedData = new float[PlayerRecorder.dataSize];

        for (var n = 0; n < PlayerRecorder.dataSize; n++)
        {
            var y = yList[n];

            if (n == 4 || n == 5)
            {
                y = CorrectAngles(y);
            }

            interpolatedData[n] = LagrangeInterpolation(x, y, pastTime);
        }

        return interpolatedData;

        // - inner function
        static int[] GetIndexes(List<float[]> dataList, float pastTime)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return null;
            }

            var listLength = dataList.Count;

            for (var n = 0; n < listLength; n++)
            {
                if (dataList[n][0] < pastTime) { continue; }

                return Indexes(n, interpolationSize, listLength);
            }

            var lastIndex = dataList.Count - 1;
            return new int[1] { lastIndex };

            // - - inner function
            static int[] Indexes(int n, int halfLength, int listLength)
            {
                var minIndex = n - halfLength - 1;
                var maxIndex = n + halfLength;

                if (minIndex < 0) { minIndex = 0; }
                if (maxIndex > listLength - 1) { maxIndex = listLength - 1; }

                var length = maxIndex - minIndex + 1;
                var indexes = new int[length];

                var counter = 0;

                for(var i = minIndex; i < maxIndex + 1; i++)
                {
                    indexes[counter] = i;
                    counter++;
                }

                return indexes;
            }
        }

        // - inner function
        static float[] GetX(List<float[]> dataList, int[] indexes)
        {
            var x = new float[indexes.Length];

            for(var n = 0; n < indexes.Length; n++)
            {
                var idx = indexes[n];
                x[n] = dataList[idx][0];
            }

            return x;
        }

        // - inner function
        static List<float[]> GetYList(List<float[]> dataList, int[] indexes)
        {
            var yList = new List<float[]>();

            for (var n = 0; n < PlayerRecorder.dataSize; n++)
            {
                var y = new float[indexes.Length];

                for (var m = 0; m < indexes.Length; m++)
                {
                    var idx = indexes[m];
                    y[m] = dataList[idx][n];
                }

                yList.Add(y);
            }

            return yList;
        }

        // - inner function
        static float[] CorrectAngles(float[] angles)
        {
            if (angles.Length == 1) { return angles; }

            var needCorrection = false;

            for(var n = 0; n < angles.Length - 1; n++)
            {
                if (angles[n] > 300.0f && angles[n + 1] < 100.0f) { needCorrection = true; break; }
                if (angles[n] < 100.0f && angles[n + 1] > 300.0f) { needCorrection = true; break; }
            }

            if (!needCorrection) { return angles; }

            for(var n = 0; n < angles.Length; n++)
            {
                if (angles[n] < 100.0f)
                {
                    angles[n] += 360.0f;
                }
            }

            return angles;
        }
    }

    static float LagrangeInterpolation(float[] x, float[] y, float p)
    {
        if (x.Length != y.Length) { return SimpleInterpolation(y); }

        var length = x.Length;
        if (length == 0) { return 0.0f; }
        if (length == 1) { return SimpleInterpolation(y); }

        var sum = 0.0f;

        for(var i = 0; i < length; i++)
        {
            var a = 1.0f;
            var b = 1.0f;

            for(var j = 0; j < length; j++)
            {
                if (i == j) { continue; }

                a *= (p - x[j]);
                b *= (x[i] - x[j]);
            }

            if (b == 0.0f) { return SimpleInterpolation(y); }
            sum += (a / b) * y[i];
        }

        return sum;

        // - inner function
        static float SimpleInterpolation(float[] y)
        {
            var sum = 0.0f;

            for(var n = 0; n < y.Length; n++)
            {
                sum += y[n];
            }

            return sum / y.GetLength(0);
        }
    }
}
