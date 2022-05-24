using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTiles : MonoBehaviour
{
    static GameObject _tile;

    void Start()
    {
        _tile = Resources.Load<GameObject>("Debug/Tile");

        for (var n = 0; n < 28; n++)
        {
            var angle = -3.0f * (n + 1);

            var z = 5.0f;
            var x = -28.0f + 2.0f * n - 1.0f;

            var tile = Object.Instantiate(_tile, new Vector3(x, 0.0f, z), Quaternion.Euler(new Vector3(angle, 0.0f, 0.0f)));
            tile.transform.localScale = new Vector3(2.0f, 1.0f, 5.0f);

            tile.transform.SetParent(gameObject.transform);
        }
    }

    void Update()
    {
        
    }
}
