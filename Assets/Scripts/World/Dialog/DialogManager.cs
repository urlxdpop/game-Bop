using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField] private GameObject _dialogBox;
    [SerializeField] private Text _dialogText;
    [SerializeField] private int _lettersForSecond = 20;

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
        if ((Input.anyKeyDown && ArrowAndWASDDontKeyDown()) && !_isTyping) {
            _currentLine++;
            if (_currentLine < _dialog.Lines.Count) {
                StartCoroutine(TypeDialog(_dialog.Lines[_currentLine]));
            } else { 
                _dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
                _currentLine = 0;
            }
        }
    }

    public IEnumerator ShowDialog(Dialog dialog) {
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();

        _dialogBox.SetActive(true);
        _dialog = dialog;

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

    private bool ArrowAndWASDDontKeyDown() {
        return !(Input.GetKeyDown(KeyCode.DownArrow) 
            || Input.GetKeyDown(KeyCode.UpArrow) 
            || Input.GetKeyDown(KeyCode.LeftArrow) 
            || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.D)
            );
    }
}
