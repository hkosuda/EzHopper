using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyerUiText : MonoBehaviour
{
    static float best = 0.0f;

    static Text maxZ;
    static Text prevZ;

    private void Awake()
    {
        maxZ = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        prevZ = gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();

        maxZ.text = "最高記録：";
        prevZ.text = "前回の記録：";
    }

    private void Start()
    {
        UpdateContent(null, new Vector3(0.0f, 0.0f, best));

        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    static void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            InvalidArea.CourseOut += UpdateContent;
        }

        else
        {
            InvalidArea.CourseOut -= UpdateContent;
        }
    }

    static void UpdateContent(object ojb, Vector3 pos)
    {
        if (InGameTimer.Paused) { return; }

        var z = pos.z;

        prevZ.text = "前回の記録：" + z.ToString("F2");

        if (z > best)
        {
            best = z;
        }

        maxZ.text = "最高記録：" + best.ToString("F2");
    }
}
