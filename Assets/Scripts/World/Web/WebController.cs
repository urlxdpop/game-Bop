using UnityEngine;

[SelectionBase]
public class WebController : MonoBehaviour, IEvent {
    private bool _isDestroy = false;

    public void Interact() {
        if (!_isDestroy) {
            _isDestroy = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    public bool IsDestroy() {
        return _isDestroy;
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
