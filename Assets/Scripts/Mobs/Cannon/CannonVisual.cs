using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CannonVisual : MonoBehaviour {
    private Animator _animator;

    private const string IS_DIE = "IsDie";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void DestroyObj() {
        GetComponentInParent<CannonController>().DestroyYourself();
        Destroy(gameObject);
    }

    public void Die() {
        _animator.SetBool(IS_DIE, true);
    }
}

