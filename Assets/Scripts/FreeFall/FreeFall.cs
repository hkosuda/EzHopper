using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFall : MonoBehaviour
{
    [SerializeField] GameObject _respawnPosition;
    [SerializeField] GameObject _dropHole;

    [SerializeField] float initialAngle = 90.0f;
    [SerializeField] float angleDelta = 90.0f;

    GameObject respawnPosition;
    GameObject dropHole;

    GameObject _triplePanel;
    GameObject myself;

    private void Awake()
    {
        myself = gameObject;
        _triplePanel = Resources.Load<GameObject>("FreeFall/TriplePanel");

        respawnPosition = _respawnPosition;
        dropHole = _dropHole;
    }

    void Start()
    {
        if (respawnPosition == null){ Debug.Log("No Respawn Position"); }
        if (dropHole == null) { Debug.Log("No Drop Hole"); }

        DeployPanels();
    }

    private void Update()
    {
        if (PM_Main.Myself.transform.position.y < dropHole.transform.position.y)
        {
            PM_Main.Myself.transform.position = respawnPosition.transform.position;
        }
    }

    void DeployPanels()
    {
        var pos = gameObject.transform.position;

        var y = 0.0f;
        var rotY = initialAngle;

        for(var n = 0; n < 30; n++)
        {
            var panel = Instantiate(_triplePanel);
            panel.transform.SetParent(myself.transform);

            panel.transform.position = new Vector3(pos.x, y, pos.z);
            panel.transform.eulerAngles = new Vector3(0.0f, rotY, 0.0f);

            SetRespawnPosition(panel);

            y += (-15.0f - 1.5f * n);
            rotY += angleDelta;
        }

        y -= 15.0f;
        dropHole.transform.position = new Vector3(pos.x, y, pos.z);

        // - inner function
        void SetRespawnPosition(GameObject panel)
        {
            Set(panel, 0);
            Set(panel, 1);
            Set(panel, 2);

            // - inner function
            void Set(GameObject panel, int n)
            {
                panel.transform.GetChild(n).gameObject.GetComponent<InvalidArea>().SetRespawnPosition(respawnPosition);
            }
        }
    }
}
