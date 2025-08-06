using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour {
    [SerializeField] private Image[] _lives;
    [SerializeField] private Sprite _fullLive;
    [SerializeField] private Sprite _emptyLive;

    private void Start() {
        VisualMaxHP();
    }

    private void Update() {
        VisualCurrentHP();
    }

    private void VisualMaxHP() {
        for (int i = 0; i < _lives.Length; i++) {
            if (i < Player.Instance.GetMaxHP()) {
                _lives[i].enabled = true;
            } else {
                _lives[i].enabled = false;
            }
        }
    }

    private void VisualCurrentHP() {
        for (int i = 0; i < _lives.Length; i++) {
            if (i < Player.Instance.GetHP()) {
                _lives[i].sprite = _fullLive;
            } else {
                _lives[i].sprite = _emptyLive;
            }
        }
    }
}
