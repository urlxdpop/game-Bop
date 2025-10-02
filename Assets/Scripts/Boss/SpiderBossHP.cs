using UnityEngine;
using UnityEngine.UI;

public class SpiderBossHP : MonoBehaviour {
    [SerializeField] private Image[] _lives;
    [SerializeField] private Sprite _fullLive;
    [SerializeField] private Sprite _emptyLive;

    private SpiderBossController _spiderBoss;

    private void Start() {
        _spiderBoss = GetComponent<SpiderBossController>();
        VisualMaxHP();
    }

    private void Update() {
        VisualCurrentHP();
    }

    private void VisualMaxHP() {
        for (int i = 0; i < _lives.Length; i++) {
            if (i < _spiderBoss.GetHP()) {
                _lives[i].enabled = true;
            } else {
                _lives[i].enabled = false;
            }
        }
    }

    private void VisualCurrentHP() {
        for (int i = 0; i < _lives.Length; i++) {
            if (i < _spiderBoss.GetHP()) {
                _lives[i].sprite = _fullLive;
            } else {
                _lives[i].sprite = _emptyLive;
            }
        }
    }
}
