using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private LevelData _levelData;
    [SerializeField] private SecretsData _secretData;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Button _playerName;
    [SerializeField] private Button _loginButton;

    private void Start() {
        UpdatePlayerData();
    }

    public void StartGame() {
        SceneManager.LoadScene(_levelData.levelName);
    }

    public void UpdatePlayerData() {
        _levelData.Load();
        _levelData.SetLevelName();
        _secretData.Load();
        _playerData.Load();

        if (_playerData.playerId != -1) {
            _playerName.GetComponentInChildren<Text>().text = _playerData.playerName;
            _loginButton.gameObject.SetActive(false);
            _playerName.gameObject.SetActive(true);
        } else {
            _loginButton.gameObject.SetActive(true);
            _playerName.gameObject.SetActive(false);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void Logout() {
        _playerData.Logout();
        SceneManager.LoadScene("Menu");
    }

    public void ResetData() {
        _levelData.ResetData();
        _secretData.ResetData();

        SceneManager.LoadScene("Menu");
    }

    public void OpenAllLevels() {
        for (int i = 0; i < _levelData.levelOpened.Length; i++) {
            _levelData.levelOpened[i] = true;
        }

        _levelData.Save();

        SceneManager.LoadScene("Menu");
    }
}
