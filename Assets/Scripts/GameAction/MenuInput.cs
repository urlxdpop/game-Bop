using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private InputAction pauseAction;

    private bool _isPaused;
    private bool _isPlaying = true;

    private const string MenuScene = "MainMenu";

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }

    private void Update()
    {
        if (pauseAction.triggered && _isPlaying)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (_isPaused)
            Resume();
        else
            Pause();
    }

    public void Final()
    {
        _isPlaying = false;
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
        AudioListener.pause = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(MenuScene);
    }
}

