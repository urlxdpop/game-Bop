using UnityEngine;

public class ClaimCamera : MonoBehaviour
{
    private void Update()
    {
        ClampToCamera();
    }

    private void ClampToCamera()
    {
        Camera cam = Camera.main;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        Vector3 pos = transform.position;

        Vector3 camPos = cam.transform.position;

        pos.x = Mathf.Clamp(pos.x, camPos.x - width, camPos.x + width);
        pos.y = Mathf.Clamp(pos.y, camPos.y - height, camPos.y + height);

        transform.position = pos;
    }
}
