using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {
    [SerializeField] private GameObject _LevelButton;

    private string[] _allLevels;
    private GameObject[] _buttons = new GameObject[10];

    private void Start() {
        _allLevels = SetLevels();
        CreateLevelButtons();
    }

    private string[] SetLevels() {
        return new string[] { "1-1", "1-2", "1-3"};
    }

    private void CreateLevelButtons() {
        for (int i = 0; i < 3; i++) {
            _buttons[i] = Instantiate(_LevelButton, new Vector3(transform.position.x, transform.position.y - 50*i, 0), transform.rotation, transform);
            _buttons[i].name = _allLevels[i];
            _buttons[i].GetComponent<LevelButton>().SetData(_allLevels[i]);
        }
    }
}
