using TMPro;
using UnityEngine;

public class EventDialogController : MonoBehaviour, IEvent
{
    [SerializeField] private int _numberEvent;
    [SerializeField] private TextMeshProUGUI _textNumEvent;
    [SerializeField] private Dialog _dialog;

    private bool _isActive;

    private void OnValidate() {
        _textNumEvent.text = _numberEvent.ToString();
    }

    private void Awake() {
        _isActive = true;
    }

    public int NumberEvent() {
        return _numberEvent;
    }

    public void SetDialog(Dialog dialog) {
        _dialog = dialog;
    }

    public bool IsDialog() {
        return _dialog.Speakers != null && _dialog.Speakers.Count != 0;
    }

    public void Interact() {
        if (_isActive) {
            if (IsDialog()) {
                StartCoroutine(DialogManager.Instance.ShowDialog(_dialog, _numberEvent));
                _isActive = false;
            }else {
                FindDialog();
            }
        }
    }

    private void FindDialog() {
        EventDialogController[] eventDialogs = FindObjectsByType<EventDialogController>(FindObjectsSortMode.None);

        foreach (EventDialogController eventDialog in eventDialogs) {
            if (eventDialog != this && eventDialog.NumberEvent() == _numberEvent) {
                if (eventDialog.IsDialog()) {
                    eventDialog.Interact();
                    break;
                }
                
            }
        }
    }

}
