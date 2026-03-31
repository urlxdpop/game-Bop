using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private float baseOrthoSize = 5f;
    [SerializeField] private float targetAspect = 16f / 9f;

    void Update()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        if (currentAspect >= targetAspect)
            Camera.main.orthographicSize = baseOrthoSize;
        else
            Camera.main.orthographicSize = baseOrthoSize * (targetAspect / currentAspect);
    }
}