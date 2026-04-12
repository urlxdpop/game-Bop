using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private float _rightLimit, _leftLimit, _topLimit, _bottomLimit;
    private bool _limitsReady;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void SetLimit(float right, float left, float bottom, float top)
    {
        if (_limitsReady) return;

        float halfH = _camera.orthographicSize;
        float halfW = halfH * (16f / 9f);

        _rightLimit = right - halfW;
        _leftLimit = left + halfW;
        _bottomLimit = bottom + halfH;
        _topLimit = top - halfH;

        _limitsReady = true;
    }

    private void LateUpdate()
    {
        if (!_limitsReady) return;

        Vector3 target = Player.Instance.transform.position;
        transform.position = new Vector3(
            Mathf.Clamp(target.x, _leftLimit, _rightLimit),
            Mathf.Clamp(target.y, _bottomLimit, _topLimit),
            transform.position.z
        );
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!_limitsReady) return;
        Gizmos.color = Color.cyan;
        Vector3 center = new Vector3(
            (_leftLimit + _rightLimit) / 2f,
            (_bottomLimit + _topLimit) / 2f,
            transform.position.z
        );
        Vector3 size = new Vector3(_rightLimit - _leftLimit, _topLimit - _bottomLimit, 0);
        Gizmos.DrawWireCube(center, size);
    }
#endif
}