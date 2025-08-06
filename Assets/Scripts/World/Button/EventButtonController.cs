using TMPro;
using UnityEngine;

[SelectionBase]
public class EventButtonController : MonoBehaviour, IEvent {
    [SerializeField] private int _numberEvent;
    [SerializeField] private bool _isActive;
    [SerializeField] private TextMeshProUGUI _textNumEvent;
    [SerializeField] private LayerMask _foregroundLayer;

    private bool _startActiveState;
    private GameObject _box;
    private bool _boxActivated;

    private void OnValidate() {
        _textNumEvent.text = _numberEvent.ToString();
    }

    private void Start() {
        _startActiveState = _isActive;
    }

    private void Update() {
        _boxActivated = CheckBox();
        CheckActive();

        
    }

    public void Interact() {
        _isActive = !_isActive;

        EventTriggerActivated();
    }

    public bool IsActive() {
        return _isActive;
    }

    private void CheckActive() {
        if (_isActive != _startActiveState) {
            if (gameObject != Player.Instance.CurrentInteractableObject() && !_boxActivated) {
                _isActive = _startActiveState;
                EventTriggerDeactivated();
            }
        }
    }

    private void EventTriggerActivated() {
        IBlockEvent[] events = EventController.Instance.GetAllEvents();

        foreach (IBlockEvent e in events) {
            if (e.NumberEvent() == _numberEvent) {
                e.Activated();
            }
        }
    }

    private void EventTriggerDeactivated() {
        IBlockEvent[] events = EventController.Instance.GetAllEvents();

        foreach (IBlockEvent e in events) {
            if (e.NumberEvent() == _numberEvent) {
                e.Deactivated();
            }
        }
    }

    private bool CheckBox() {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), _foregroundLayer);

        if (collider.gameObject.GetComponent<BoxController>() || collider.gameObject.GetComponent<SpiderController>()) {
            if (collider.gameObject != _box) {
                _box = collider.gameObject;
                _isActive = !_isActive;
                EventTriggerActivated();
                return true;
            } else {
                return true;
            }
        }
        _box = null;
        return false;
    }
}
