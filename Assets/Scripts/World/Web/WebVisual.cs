using UnityEngine;

public class WebVisual : MonoBehaviour {
    [SerializeField] private WebController _block;

    private Animator _animator;

    private bool _isDestroy;

    private const string IS_DESTROY = "IsDestroy";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _isDestroy = _block.IsDestroy();

        _animator.SetBool(IS_DESTROY, _isDestroy);
    }

    public void Destroy() {
        _block.Destroy();
    }
}
