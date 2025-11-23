using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {
    [SerializeField] private GameObject _LevelButton;

    private string[] _allLevels;
    private GameObject[] _buttons = new GameObject[10];
    private int _numLevels = 0;
    private int _page = 0;

    private const int PAGES = 2;

    private void Start() {
        _allLevels = SetLevels();
        CreateLevelButtons();
    }


    public void NextPage() {
        _page++;
        if(_page >= PAGES) {
            _page = 0;
        }
        CreateLevelButtons();
    }

    public void LastPage() {
        _page--;
        if (_page < 0) {
            _page = PAGES - 1;
        }
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
        for (int i = 0; i < 10; i++) {
            if (_buttons[i] != null) {
                Destroy(_buttons[i]);
            }
        }
        for (int i = _page * 10; i < (_page + 1) * 10; i++) {
            if (_allLevels[i] == null) break;
            int j = i - _page * 10;
            _buttons[j] = Instantiate(_LevelButton, new Vector3(transform.position.x, transform.position.y - 75*j, 0), transform.rotation, transform);
            _buttons[j].name = _allLevels[i];
            _buttons[j].GetComponent<LevelButton>().SetData(_allLevels[i]);
        }
    }

}
