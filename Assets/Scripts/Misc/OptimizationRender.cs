using System.Collections.Generic;
using UnityEngine;

public class OptimizationRender : MonoBehaviour
{

    private Camera mainCamera;
    private SpriteRenderer[] objects;

    private void Start()
    {
        objects = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            return;
        }

        Vector3 min = mainCamera.ViewportToWorldPoint(Vector3.zero);
        Vector3 max = mainCamera.ViewportToWorldPoint(Vector3.one);

        foreach (var obj in objects)
        {
            if (GetComponentInParent<PlayerSpawner>()) return;
            Vector3 pos = obj.transform.position;
            bool visible = pos.x >= min.x - 0.5f && pos.x <= max.x + 0.5f &&
                           pos.y >= min.y - 0.5f && pos.y <= max.y + 0.5f;
            obj.enabled = visible;
        }
    }

}
