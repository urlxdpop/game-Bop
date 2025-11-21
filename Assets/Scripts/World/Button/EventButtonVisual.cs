
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EventButtonVisual : MonoBehaviour {
    
    [SerializeField] private EventButtonController _button;

    private Animator _animator;

    private const string ACTIVE = "Active";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        GetComponentInChildren<Canvas>().enabled = false;
    }

    private void Update() {
        _animator.SetBool(ACTIVE, _button.IsActive());
    }
}
