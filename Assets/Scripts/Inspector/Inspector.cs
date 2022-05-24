using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inspector : MonoBehaviour
{
    enum InspectorItem
    {
        pm_landing,
        pm_vector_magnitude,
        pm_surface_normal_vector,
        pm_input_vector,
        pm_plane_vector,
        pm_clip_vector,

    }

    static Dictionary<InspectorItem, string> ItemValueList;

    static TextMeshProUGUI itemsText;
    static TextMeshProUGUI valuesText;

    void Start()
    {
        itemsText = gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        valuesText = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        ItemValueList = new Dictionary<InspectorItem, string>();

        foreach(InspectorItem item in Enum.GetValues(typeof(InspectorItem)))
        {
            ItemValueList.Add(item, "");
        }
    }

    void Update()
    {
        UpdateContent();
        UpdateText();
    }

    static void UpdateContent()
    {
        PmLanding();
        PmVectorMagnitude();
        PmInputVector();
        PmPlaneVector();

        // - inner function
        static void PmVectorMagnitude()
        {
            ItemValueList[InspectorItem.pm_vector_magnitude] = PM_Main.Rb.velocity.magnitude.ToString("f2");
        }

        // - inner function
        static void PmLanding()
        {
            var indicator = PM_Landing.LandingIndicator;

            if (indicator < 0)
            {
                ItemValueList[InspectorItem.pm_landing] = "<color=red>" + indicator.ToString() + "</color>";
                return;
            }

            if (indicator > 0)
            {
                ItemValueList[InspectorItem.pm_landing] = "<color=green>" + indicator.ToString() + "</color>";
            }

            else
            {
                ItemValueList[InspectorItem.pm_landing] = "<color=blue>" + indicator.ToString() + "</color>";
            }
        }

        // - inner function
        static void PmInputVector()
        {
            var vec = PM_InputVector.InputVector;

            ItemValueList[InspectorItem.pm_input_vector] = vec.x.ToString("f2") + ",\t" + vec.y.ToString("f2");
        }

        // - inner function
        static void PmPlaneVector()
        {
            var vec = PM_PlaneVector.PlaneVector;

            ItemValueList[InspectorItem.pm_plane_vector] = vec.x.ToString("f2") + ",\t00.00,\t" + vec.y.ToString("f2");
        }
    }

    static void UpdateText()
    {
        itemsText.text = "";
        valuesText.text = "";

        foreach(var itemValue in ItemValueList)
        {
            itemsText.text += itemValue.Key.ToString() + "\n";
            valuesText.text += itemValue.Value.ToString() + "\n";
        }
    }
}
