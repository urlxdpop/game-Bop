using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private float _rightLimit;
    private float _leftLimit;
    private float _topLimit;
    private float _bottomLimit;

    public void SetLimit(float right, float left, float bottom, float top) {
        float orthographicSize = _camera.orthographicSize;
        float aspectRatio = _camera.aspect;

        _rightLimit = right - orthographicSize * aspectRatio - 0.3f;
        _leftLimit = left + orthographicSize * aspectRatio + 1.3f;
        _bottomLimit = bottom + orthographicSize + 1.2f;
        _topLimit = top - orthographicSize - 0.3f;
    }

    private void LateUpdate() {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(Player.Instance.transform.position.x, _leftLimit, _rightLimit);
        newPosition.y = Mathf.Clamp(Player.Instance.transform.position.y, _bottomLimit, _topLimit);
        transform.position = newPosition;
    }
}
