using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

enum DialogAutors {
    Повестователь,
    Гэп,
    Неизвестный,
    Подсказка,
    Табличка
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private GameObject _dialogBox;
    [SerializeField] private Text _dialogAutor;
    [SerializeField] private Text _dialogText;
    [SerializeField] private int _lettersForSecond = 20;
    [SerializeField] private InputAction _skipDialog;

    public event Action OnShowDialog;
    public event Action OnHideDialog;

    private int _currentLine;
    private Dialog _dialog;
    private bool _isTyping;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _currentLine = 0;

        _dialogBox.SetActive(false);
    }

    public void HandleUpdate() {
        if (_skipDialog.triggered && !_isTyping) {
            _currentLine++;
            if (_currentLine < _dialog.Lines.Count) {
                _dialogAutor.text = _dialog.Speakers[_currentLine].ToString();
                StartCoroutine(TypeDialog(_dialog.Lines[_currentLine]));
            } else { 
                _dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
                _currentLine = 0;
                
            }
        }
    }

    public IEnumerator ShowDialog(Dialog dialog, int numEvent) {
        if (EventController.Instance.IsActivatedEvent(numEvent)) {
            yield break;
        } else {
            EventController.Instance.Activate(numEvent);
        }

        yield return new WaitForEndOfFrame();
        
        OnShowDialog?.Invoke();

        _dialogBox.SetActive(true);
        _dialog = dialog;
        _dialogAutor.text = _dialog.Speakers[0].ToString();

        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    private IEnumerator TypeDialog(string line) {
        _isTyping = true;
        _dialogText.text = "";

        foreach (char letter in line.ToCharArray()) {
            _dialogText.text += letter;

            yield return new WaitForSeconds(1 / _lettersForSecond);
        }

        _isTyping = false;
    }

    private void OnEnable()
    {
        _skipDialog.Enable();
    }

    private void OnDisable()
    {
        _skipDialog.Disable();
    }
}
