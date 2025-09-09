using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour {
    [SerializeField] private Image[] _oxygen;
    [SerializeField] private Sprite _fullOxygenSprite;
    [SerializeField] private Sprite _emptyOxygenSprite;

    private void Start() {
        VisualMaxOxygen();
    }

    private void Update() {
        VisualCurrentOxygen();
    }

    private void VisualMaxOxygen() {
        for (int i = 0; i < _oxygen.Length; i++) {
            if (i < 5) {
                _oxygen[i].enabled = true;
            } else {
                _oxygen[i].enabled = false;
            }
        }
    }

    private void VisualCurrentOxygen() {
        for (int i = 0; i < _oxygen.Length; i++) {
            if (i < Player.Instance.GetOxygen()) {
                _oxygen[i].sprite = _fullOxygenSprite;
            } else {
                _oxygen[i].sprite = _emptyOxygenSprite;
            }
        }
    }
}
