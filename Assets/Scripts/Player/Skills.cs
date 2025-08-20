using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Skills : MonoBehaviour {
    [SerializeField] private LayerMask _foreground;

    private bool _isActive;
    private Vector3 _animPos;

    private Animator _animator;

    private const string DESTROY = "Destroy";
    private const string MAGNET = "Magnet";


    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        if (_isActive) {
            transform.position = _animPos;
        }
    }

    public void DestroyYourself() {
        _animator.SetTrigger(DESTROY);
        _isActive = true;
        _animPos = Player.Instance.CurrentPos();
    }

    public void Magnet() {
        _animator.SetTrigger(MAGNET);
        _isActive = true;
        _animPos = Player.Instance.CurrentPos();
    }

    public void CloseAnim() {
        _isActive = false;
    }

    public bool IsActive() {
        return _isActive;
    }
}
