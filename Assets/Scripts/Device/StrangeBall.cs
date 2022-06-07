using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeBall : MonoBehaviour
{
    [SerializeField] float x = 0.0f;
    [SerializeField] float minY = 0.0f;
    [SerializeField] float maxY = 1.0f;
    [SerializeField] float minZ = 0.0f;
    [SerializeField] float maxZ = 1.0f;

    private void Awake()
    {
        UpdatePosition();
    }

    void Start()
    {
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
            DE_Shooter.ShootingHit += React;
        }

        else
        {
            DE_Shooter.ShootingHit -= React;
        }
    }

    void React(object obj, RaycastHit hit)
    {
        if (hit.collider.gameObject == gameObject)
        {
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        var y = UnityEngine.Random.Range(minY, maxY);
        var z = UnityEngine.Random.Range(minZ, maxZ);

        gameObject.transform.localPosition = new Vector3(x, y, z);
    }
}
