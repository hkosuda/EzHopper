using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoData
{
    public string filename;

    public DemoData(string filename)
    {
        this.filename = filename;
    }

    public List<float[]> DataList()
    {
        var asset = Resources.Load<TextAsset>("DemoData/" + filename + ".ghost");
        if (asset == null) { return new List<float[]>(); }

        var dataList = DemoUtils.FullText2DataList(asset.text);

        return dataList;
    }
}
