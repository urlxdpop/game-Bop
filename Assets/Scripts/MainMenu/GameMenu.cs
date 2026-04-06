using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Text _time;
    [SerializeField] private InputAction pauseAction;
    [SerializeField] private InputAction restartAction;
    [SerializeField] private InputAction quitAction;
    
    private bool _isPaused;
    private bool _isPlaying = true;

    private const string MenuScene = "Menu";

    private void Start()
    {
        pauseAction.Enable();
        restartAction.Enable();
        quitAction.Enable();
    }

    private void Update()
    {
        float time = GameController.Instance.GameTime();
        _time.text = Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString();

        if (GameController.Instance.IsDialogOpen) return;
        if (pauseAction.triggered && _isPlaying)
        {
            TogglePause();
        } else if (restartAction.triggered && _isPaused) {
            Restart();
        } else if (quitAction.triggered && _isPaused) {
            OpenMainMenu();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        _isPaused = false;
        PauseAndDisableInput();
        Player.Instance.Die();
    }

    public void Final()
    {
        _isPlaying = false;
    }

    public void Continue()
    {
        GameController.Instance.ResumeGame();
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        PauseAndDisableInput();
        DOTween.KillAll();
        SceneManager.LoadScene(MenuScene);
    }

    public void LoadMenu()
    {
        if (_isPlaying)
        {
            GameController.Instance.OpenMenu();
            Time.timeScale = 0f;
            _isPaused = true;
        }
    }

    private void TogglePause()
    {
        if (_isPaused)
            Continue();
        else
            LoadMenu();
    }

    private void PauseAndDisableInput()
    {
        pauseAction.Disable();
        restartAction.Disable();
        quitAction.Disable();
    }

    private void OnDisable()
    {
        PauseAndDisableInput();
    }
}
