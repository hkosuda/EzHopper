using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // public
    static public float[] InterpolatedData { get; private set; }
    static public bool DemoMode { get; private set; }

    // params
    static List<float[]> dataList;
    static float pastTime;

    // objects
    static GameObject _ghost;
    static GameObject _ghostLine;

    static GameObject ghost;
    static GameObject ghostBody;

    static GameObject ghostLineObject;
    static LineRenderer ghostLine;

    private void Awake()
    {
        ghostBody = gameObject.transform.GetChild(0).gameObject;
    }

    void Start()
    {
        SetEvent(1);
    }

    private void OnDestroy()
    {
        DemoMode = false;
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Timer.Updated += UpdateMethod;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (dataList == null || dataList.Count == 0) { return; }

        pastTime += dt;
        if (pastTime > dataList.Last()[0]) { Repeat(); return; }

        InterpolatedData = Interpolate();

        if (DemoMode) { SetVisibility(false); return; }
        if (PlayerRecorder.RecordSize() > 2) { SetVisibility(false); return; }

        SetVisibility(true);
        UpdateTransform();
        UpdateLine();

        // - inner function
        static void SetVisibility(bool visibility)
        {
            ghostBody.SetActive(visibility);
            ghostLineObject.SetActive(visibility);
        }
    }

    static void UpdateTransform()
    {
        ghost.transform.position = Vec3(InterpolatedData);
    }

    static void UpdateLine()
    {
        ghostLine.positionCount++;
        ghostLine.SetPosition(ghostLine.positionCount - 1, Vec3(InterpolatedData, -PM_Main.centerY));
    }

    static public void BeginReplay(List<float[]> _dataList, bool _demoMode)
    {
        EndReplay();

        if (_ghost == null) { _ghost = Resources.Load<GameObject>("Ghost/Ghost"); }
        if (_ghostLine == null) { _ghostLine = Resources.Load<GameObject>("Ghost/GhostLine"); }

        DemoMode = _demoMode;
        dataList = _dataList;
        pastTime = 0.0f;

        if (dataList == null || dataList.Count == 0) { return; }

        ghost= Object.Instantiate(_ghost);
        ghost.transform.position = Vec3(dataList[0]);

        ghostLineObject = Object.Instantiate(_ghostLine);
        ghostLine = ghostLineObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();

        ghostLine.positionCount = 1;
        ghostLine.SetPosition(0, Vec3(dataList[0], -PM_Main.centerY));
    }

    static void EndReplay()
    {
        if (ghost == null) { return; }

        Destroy(ghost);
        Destroy(ghostLineObject);
    }

    static void Repeat()
    {
        if (DemoMode) 
        {
            EndReplay();
            DemoMode = false;

            dataList = null;
            return;
        }

        if (dataList != null && dataList.Count > 0)
        {
            ghostLine.positionCount = 1;
            ghostLine.SetPosition(0, Vec3(dataList[0], -PM_Main.centerY));
        }

        else
        {
            ghostLine.positionCount = 1;
            ghostLine.SetPosition(0, Vector3.zero);
        }
        
        pastTime = 0.0f;
    }

    //
    // utilities
    static Vector3 Vec3(float[] data, float dy = 0.0f)
    {
        return new Vector3(data[1], data[2] + dy, data[3]);
    }

    static float[] Interpolate()
    {
        if (dataList == null || dataList.Count == 0) { return new float[2] { 0.0f, 0.0f }; }

        var indexes = GetIndexes();
        if (indexes[0] == indexes[1]) { return dataList[indexes[0]]; }

        var rate = GetRate(indexes);
        var interpolatedData = new float[PlayerRecorder.dataSize];

        for(var n = 0; n < PlayerRecorder.dataSize; n++)
        {
            interpolatedData[n] = dataList[indexes[0]][n] * rate[0] + dataList[indexes[1]][n] * rate[1];
        }

        return interpolatedData;

        // - inner function
        static int[] GetIndexes()
        {
            if (dataList == null || dataList.Count == 0)
            {
                return new int[2] { 0, 0 };
            }

            for(var n = 0; n < dataList.Count; n++)
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
        static float[] GetRate(int[] indexes)
        {
            if (indexes[0] == indexes[1]) { return new float[2] { 0.5f, 0.5f }; }

            var t1 = dataList[indexes[0]][0];
            var t2 = dataList[indexes[1]][0];

            var dt = t2 - t1;

            var rate1 = Calcf.SafetyDiv(t2 - pastTime, dt, 0.0f);
            var rate2 = 1.0f - rate1;

            return new float[2] { rate1, rate2 };
        }
    }
}
