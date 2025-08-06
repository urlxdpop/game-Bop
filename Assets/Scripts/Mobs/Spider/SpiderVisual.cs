
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class SpiderVisual : MonoBehaviour {
    [SerializeField] private SpiderController _spider;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private const string IS_DIE = "IsDie";

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Destroy() {
        _spriteRenderer.enabled = false;
        _spider.DestroyYourself();
        
    }

    public void Spider_OnDie() {
        _animator.SetTrigger(IS_DIE);
    }


}
