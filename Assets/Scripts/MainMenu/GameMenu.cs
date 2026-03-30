using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Text _time;

    [SerializeField] private InputAction pauseAction;

    private bool _isPaused;
    private bool _isPlaying = true;

    private const string MenuScene = "Menu";

    private void Update()
    {
        float time = GameController.Instance.GameTime();
        _time.text = Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString();

        if (pauseAction.triggered && _isPlaying)
        {
            TogglePause();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        _isPaused = false;
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
        DOTween.KillAll();
        SceneManager.LoadScene(MenuScene);
    }

    public void LoadMenu()
    {
        GameController.Instance.OpenMenu();
        Time.timeScale = 0f;
        _isPaused = true;
    }

    private void TogglePause()
    {
        if (_isPaused)
            Continue();
        else
            LoadMenu();
    }

    private void OnEnable()
    {
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.Disable();
    }
}
