using UnityEngine;

[RequireComponent (typeof(Animator))]
public class CollipsibleBlockVisual : MonoBehaviour
{
    [SerializeField] private CollipsibleBlockController _block;

    private Animator _animator;

    private bool _isDestroy;

    private const string IS_DESTROY = "IsDestroy";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _isDestroy = _block.IsDestroy();

        if (_isDestroy) {
            _animator.SetBool(IS_DESTROY, _isDestroy);
            GetComponent<SpriteRenderer>().sortingLayerName = "Mobs";
        }
    }

    public void Destroy() { 

        _block.Destroy();
    }
}
