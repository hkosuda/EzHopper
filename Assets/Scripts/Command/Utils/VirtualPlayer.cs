using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPlayer : MonoBehaviour
{
    static public bool CheckLandingNow { get; private set; } = false;

    bool noCollision;
    int frameBufferRemain;

    private void Awake()
    {
        CheckLandingNow = true;
    }

    private void OnDestroy()
    {
        CheckLandingNow = false;
    }

    void Update()
    {
        frameBufferRemain--;

        if (frameBufferRemain < 0)
        {
            if (noCollision)
            {
                ObserverCommand.Land(gameObject.transform.position, PM_Camera.EulerAngles());
            }

            else
            {
                ChatMessages.SendChat("オブジェクトと重なる位置であるため，この位置に着地できません．", ChatMessages.Sender.system);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        noCollision = false;

        ChatMessages.SendChat("オブジェクトと重なる位置であるため，この位置に着地できません．", ChatMessages.Sender.system);
        Destroy(gameObject);
    }

    public void Initialize(Vector3 position)
    {
        noCollision = true;
        frameBufferRemain = 10;
    }
}
