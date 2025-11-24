using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private SecretsData _secretData;

    private void Start() {
        _levelData.Load();
        _levelData.SetLevelName();
        _secretData.Load();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(_levelData.levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetData()
    {
        _levelData.ResetData();
        _secretData.ResetData();
    }
}
