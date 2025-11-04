using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _rightLimit;
    [SerializeField] private float _leftLimit;
    [SerializeField] private float _topLimit;
    [SerializeField] private float _bottomLimit;
    [SerializeField] private Camera _camera;


    public void SetLimit(float right, float left, float bottom, float top) {
        _rightLimit = right + 0.9f;
        _leftLimit = left + 1f;
        _bottomLimit = bottom - 0.4f;
        _topLimit = top - 0.4f;
    }

    private void LateUpdate() {
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(Player.Instance.transform.position.x, _leftLimit, _rightLimit);
        newPosition.y = Mathf.Clamp(Player.Instance.transform.position.y, _bottomLimit, _topLimit);
        transform.position = newPosition;
    }
}
