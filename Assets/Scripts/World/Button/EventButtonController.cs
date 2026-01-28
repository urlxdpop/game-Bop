using TMPro;
using UnityEngine;

[SelectionBase]
public class EventButtonController : MonoBehaviour, IEvent
{
    [SerializeField] private int _numberEvent;
    [SerializeField] private TextMeshProUGUI _textNumEvent;
    [SerializeField] private bool _oneActivation;

    private bool _finalActivation = false;
    private bool _isActive = false;
    private bool _startActiveState;
    private GameObject _obj;
    private bool _boxActivated;

    private void OnValidate()
    {
        _textNumEvent.text = _numberEvent.ToString();
    }

    private void Start()
    {
        _startActiveState = false;
    }

    private void Update()
    {
        if (_finalActivation) return;

        if (_oneActivation)
        {
            if (_isActive != _startActiveState)
            {
                _finalActivation = true;
            }
        }

        _boxActivated = CheckBox();
        CheckActive();
    }

    public void Interact()
    {
        _isActive = !_startActiveState;

        EventController.Instance.EventTriggerActivated(_numberEvent);
    }

    public bool IsActive()
    {
        return _isActive;
    }

    private void CheckActive()
    {
        if (_isActive != _startActiveState)
        {
            if (gameObject != Player.Instance.CurrentInteractableObject() && !_boxActivated)
            {
                _isActive = _startActiveState;
                EventController.Instance.EventTriggerDeactivated(_numberEvent);
            }
        }
    }

    private bool CheckBox()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0);
        foreach (var collider in colliders)
        {
            if (MobsOrBox(collider))
            {
                if (collider.gameObject != _obj)
                {
                    _obj = collider.gameObject;
                    _isActive = !_startActiveState;
                    EventController.Instance.EventTriggerActivated(_numberEvent);
                    return true;
                } else
                {
                    return true;
                }
            }
        }
        _obj = null;
        return false;
    }

    private bool MobsOrBox(Collider2D collider)
    {
        if (GetComponent<MobsButton>())
        {
            if (collider.GetComponent<SpiderController>() ||
                collider.GetComponent<CannonController>() ||
                collider.GetComponent<LaserGunController>()) return true;
        } else
        {
            if (collider.GetComponent<BoxController>()) return true;
        }

        return false;
    }
}
