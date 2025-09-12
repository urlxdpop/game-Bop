using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private string _levelName;
    private Text _text;

    public void SetData(string name) {
        _levelName = name;
        _text = GetComponentInChildren<Text>();
        _text.text = name;
    }

    public void LoadLevel() {
        SceneManager.LoadScene(_levelName);
    }
}
