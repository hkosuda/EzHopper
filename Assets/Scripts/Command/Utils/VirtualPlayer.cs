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
                ChatMessages.SendChat("�I�u�W�F�N�g�Əd�Ȃ�ʒu�ł��邽�߁C���̈ʒu�ɒ��n�ł��܂���D", ChatMessages.Sender.system);
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        noCollision = false;

        ChatMessages.SendChat("�I�u�W�F�N�g�Əd�Ȃ�ʒu�ł��邽�߁C���̈ʒu�ɒ��n�ł��܂���D", ChatMessages.Sender.system);
        Destroy(gameObject);
    }

    public void Initialize(Vector3 position)
    {
        noCollision = true;
        frameBufferRemain = 10;
    }
}
