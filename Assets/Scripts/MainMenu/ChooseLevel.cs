using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {
    [SerializeField] private GameObject _LevelButton;

    private string[] _allLevels;
    private GameObject[] _buttons = new GameObject[10];
    private int _numLevels = 0;

    private void Start() {
        _allLevels = SetLevels();
        CreateLevelButtons();
    }

    private string[] SetLevels() {
        string[] levels = new string[50];

        int head = 1;
        int act = 1;
        while (true) {
            string levelName = head + "-" + act;
            if (Application.CanStreamedLevelBeLoaded(levelName)) {
                levels[(head - 1) * 15 + (act - 1)] = levelName;
                act++;
                _numLevels++;
                continue;
            } else {
                act = 1;
                head++;
                levelName = head + "-" + act;
                if (Application.CanStreamedLevelBeLoaded(levelName)) {
                    levels[(head - 1) * 15 + (act - 1)] = levelName;
                    act++;
                    _numLevels++;
                    continue;
                }else {
                    break;
                }
            }
        }

        return levels;
    }

    private void CreateLevelButtons() {
        for (int i = 0; i < _numLevels; i++) {
            _buttons[i] = Instantiate(_LevelButton, new Vector3(transform.position.x, transform.position.y - 50*i, 0), transform.rotation, transform);
            _buttons[i].name = _allLevels[i];
            _buttons[i].GetComponent<LevelButton>().SetData(_allLevels[i]);
        }
    }
}
