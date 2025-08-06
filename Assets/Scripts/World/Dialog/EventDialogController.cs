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

    private void Start() {
        _isActive = true;
    }

    public void Interact() {
        if (_isActive) {
            StartCoroutine(DialogManager.Instance.ShowDialog(_dialog));
            _isActive = false;
        }
    }
}
