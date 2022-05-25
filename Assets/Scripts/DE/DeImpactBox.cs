using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeImpactBox : MonoBehaviour
{
    static readonly float impactBoxExistTime = 0.6f;

    static GameObject _impactBox;
    static GameObject impactBoxRoot;

    static List<GameObject> impactBoxList;
    static List<float> timeList;

    private void Awake()
    {
        _impactBox = Resources.Load<GameObject>("DE/ImpactBox");
        impactBoxRoot = new GameObject("ImpactBoxRoot");

        impactBoxList = new List<GameObject>();
        timeList = new List<float>();
    }

    void Start()
    {
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
            DE_Shooter.ShootingHit += InstantiateImpactBox;
            Timer.Updated += UpdateMethod;
        }

        else
        {
            DE_Shooter.ShootingHit -= InstantiateImpactBox;
            Timer.Updated -= UpdateMethod;
        }
    }

    static void InstantiateImpactBox(object obj, RaycastHit hit)
    {
        var impactBox = Instantiate(_impactBox, hit.point, Quaternion.identity);
        impactBox.transform.SetParent(impactBoxRoot.transform);

        impactBoxList.Add(impactBox);
        timeList.Add(0.0f);
    }

    static void UpdateMethod(object obj, float dt)
    {
        for(var n = impactBoxList.Count - 1; n > -1; n--)
        {
            timeList[n] += dt;

            if (timeList[n] > impactBoxExistTime)
            {
                var box = impactBoxList[n];
                Destroy(box);

                impactBoxList.RemoveAt(n);
                timeList.RemoveAt(n);
            }
        }
    }
}
