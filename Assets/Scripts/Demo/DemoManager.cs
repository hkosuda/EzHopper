using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : IKernelManager
{
    static List<DemoData> demoDataList;

    public void Initialize()
    {
        demoDataList = new List<DemoData>()
        {
            new DemoData("ez_athletic_piles"),
            new DemoData("piles")
        };
    }

    public void Shutdown()
    {

    }

    public void Reset()
    {

    }

    static public List<float[]> GetDataList(string filename)
    {
        foreach(var demoData in demoDataList)
        {
            if (demoData.filename == filename)
            {
                return demoData.DataList();
            }
        }

        return new List<float[]>();
    }
}
