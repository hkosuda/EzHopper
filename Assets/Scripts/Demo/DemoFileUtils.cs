using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class DemoFileUtils
{
    static public MapName FullText2MapName(string fullText)
    {
        var splitted = fullText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (splitted == null) { return MapName.none; }

        var mapBegin = false;

        foreach (var _line in splitted)
        {
            var line = _line.Trim();

            if (line == "map") { mapBegin = true; continue; }
            if (mapBegin && line == "end") { return MapName.none; }

            if (!mapBegin) { continue; }

            foreach(MapName mapName in Enum.GetValues(typeof(MapName)))
            {
                if (line.ToLower() == mapName.ToString())
                {
                    return mapName;
                }
            }
        }

        return MapName.none;
    }

    static public List<float[]> FullText2DataList(string fullText)
    {
        var splitted = fullText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (splitted == null) { return new List<float[]>(); }

        var dataList = new List<float[]>();
        var valuesBegin = false;

        foreach (var _line in splitted)
        {
            var line = _line.Trim();

            if (line == "values") { valuesBegin = true; continue; }
            if (valuesBegin && line == "end") { return dataList; }

            if (!valuesBegin) { continue; }

            var data = Line2Data(line);
            if (data == null) { return new List<float[]>(); }

            dataList.Add(data);
        }

        return new List<float[]>();
    }

    static public void SaveFile(string filepath, List<float[]> dataList)
    {
        CreateDirectory();

        try
        {
            using (StreamWriter sw = new StreamWriter(filepath, false))
            {
                var content = CreateFileContent(dataList);
                sw.WriteLine(content);
            }
        }

        catch
        {
            Debug.Log("�t�@�C���̍쐬�Ɏ��s���܂����D");
        }
    }

    static string CreateFileContent(List<float[]> dataList)
    {
        var content = "";

        content += "map\n";
        content += "\t" + MapsManager.CurrentMap.MapName.ToString();
        content += "end\n\n";

        content += "values\n";
        content += DataList2DataLines(dataList);
        content += "end";

        return content;
    }

    static string DataList2DataLines(List<float[]> dataList)
    {
        var dataLines = "";

        foreach(var data in dataList)
        {
            var text = Data2Line(data);
            dataLines += "\t" + text + "\n";
        }

        return dataLines;
    }

    static string Data2Line(float[] data)
    {
        var line = "";

        for(var n = 0; n < data.Length; n++)
        {
            var value = data[n];

            if (n > 5)
            {
                line += value.ToString() + ",";
            }

            else
            {
                line += value.ToString("f8") + ",";
            }
        }

        return line;
    }

    static float[] Line2Data(string line)
    {
        var splitted = line.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var data = new float[splitted.Length];

        for(var n = 0; n < data.Length; n++)
        {
            var s = splitted[n];

            if (float.TryParse(s, out var num))
            {
                data[n] = num;
            }

            else
            {
                return null;
            }
        }

        return data;
    }

    static public string FilePath(string filename, bool addExtension)
    {
        if (addExtension) { return FileDirectory() + filename + ".ghost.txt"; }

        return FileDirectory() + filename;
    }

    static public string FileDirectory()
    {
        return Application.dataPath + "/bh_ghost/";
    }

    static void CreateDirectory()
    {
        if (!Directory.Exists(FileDirectory()))
        {
            try
            {
                Debug.Log("�t�H���_���쐬���܂����D");
                Directory.CreateDirectory(FileDirectory());
            }

            catch
            {
                Debug.Log("�t�H���_�̍쐬�Ɏ��s���܂����D");
            }
        }
    }
}
