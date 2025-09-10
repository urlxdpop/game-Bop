using UnityEngine;

public class CannonVisualBoom : MonoBehaviour
{
    private Animator _animator;

    private const string BOOM = "Boom";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Off() {
        gameObject.SetActive(false);
    }

    public void On() {
        gameObject.SetActive(true);
        _animator.SetTrigger(BOOM);
    }
}
