using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBullet : MonoBehaviour
{
    static readonly float bulletSpeed = 100.0f;
    static readonly float bulletMaxExistTime = 3.0f;

    static GameObject _bullet;

    static GameObject bullet;
    static GameObject muzzle;

    static Vector3 direction;
    static Vector3 origin;

    static float pastTime;
    static float bulletExistTime;

    private void Awake()
    {
        muzzle = gameObject;
        _bullet = Resources.Load<GameObject>("DE/Bullet");
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
            Timer.Updated += UpdateMethod;
            DE_Shooter.Shot += StartPreview;
            DE_Shooter.ShootingHit += UpdateExistTime;
        }

        else
        {
            Timer.Updated -= UpdateMethod;
            DE_Shooter.Shot -= StartPreview;
            DE_Shooter.ShootingHit -= UpdateExistTime;
        }
    }

    static void UpdateMethod(object obj, float dt)
    {
        if (bullet == null) { return; }

        pastTime += dt;
        if (pastTime > bulletExistTime) { Destroy(bullet); return; }

        bullet.transform.position = origin + direction * bulletSpeed * pastTime;
    }

    static void StartPreview(object obj, Vector3 _direction)
    {
        if (bullet != null) { Destroy(bullet); }

        direction = _direction.normalized;
        origin = muzzle.transform.position;

        pastTime = 0.0f;
        bulletExistTime = bulletMaxExistTime;

        bullet = Instantiate(_bullet, muzzle.transform.position, Quaternion.identity);

        GameSystem.SetChildOfRoot(bullet);
        SetRotation(bullet);

        // - inner function
        static void SetRotation(GameObject bullet)
        {
            var rotY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var rotX = -Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;

            bullet.transform.eulerAngles = new Vector3(rotX, rotY, 0.0f);
        }
    }

    static void UpdateExistTime(object obj, RaycastHit hit)
    {
        bulletExistTime = (hit.point - origin).magnitude / bulletSpeed;
    }
}
