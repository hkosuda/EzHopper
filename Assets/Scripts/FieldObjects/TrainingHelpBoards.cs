using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingHelpBoards : MonoBehaviour
{
    static readonly Bools.Item item = Bools.Item.show_help;

    GameObject body;
    GameObject canvas;

    private void Awake()
    {
        body = gameObject.transform.GetChild(0).gameObject;
        canvas = gameObject.transform.GetChild(1).gameObject;
    }

    void Start()
    {
        UpdateVisibility(null, false);
        SetEvent(1);
    }

    private void OnDestroy()
    {
        SetEvent(-1);
    }

    void SetEvent(int indicator)
    {
        if (indicator > 0)
        {
            Bools.Settings[item].ValueUpdated += UpdateVisibility;
        }

        else
        {
            Bools.Settings[item].ValueUpdated -= UpdateVisibility;
        }
    }

    void UpdateVisibility(object obj, bool prevValue)
    {
        var currentValue = Bools.Get(item);

        body.SetActive(currentValue);
        canvas.SetActive(currentValue);
    }
}
