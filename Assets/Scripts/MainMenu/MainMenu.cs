using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private SecretsData _secretData;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Button _playerName;
    [SerializeField] private Button _loginButton;
    [SerializeField] private InputAction _start;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = GetComponentInParent<CanvasGroup>();
            }
        }
        
        if (Application.isMobilePlatform)
        {
            if (PlayerPrefs.GetInt("FirstLaunch", 0) == 0)
            {
                PlayerPrefs.SetInt("FirstLaunch", 1);
                ResetData();
            }
            Debug.Log(PlayerPrefs.GetInt("FirstLaunch", 0) == 0);
        }

        _start.Enable();
    }

    private void Start()
    {
        StartCoroutine(StartMenu());
    }

    private IEnumerator StartMenu()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        yield return new WaitForSeconds(0.5f);
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        UpdatePlayerData();
        YG2.StickyAdActivity(true);

        if (YG2.isFirstGameSession && !_playerData.HasInitialized)
        {
            ResetData();
            _playerData.HasInitialized = true;
            _playerData.Save();
        }
    }

    private void Update()
    {
        if (_start.triggered) {
            StartGame();
        }
    }

    private void OnDestroy()
    {
        _start.Disable();
    }

    public void StartGame()
    {
        _start.Disable();
        SceneManager.LoadScene(_levelData.levelName);
    }

    public void UpdatePlayerData()
    {
        _levelData.Load();
        _levelData.SetLevelName();
        _secretData.Load();
        _playerData.Load();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Logout()
    {
        _playerData.Logout();
        _start.Disable();
        SceneManager.LoadScene("Menu");
    }

    public void ResetData()
    {
        _levelData.ResetData();
        _secretData.ResetData();
        _start.Disable();
        SceneManager.LoadScene("Menu");
    }

    public void OpenAllLevels()
    {
        for (int i = 0; i < _levelData.levelOpened.Length; i++)
        {
            _levelData.levelOpened[i] = true;
        }

        _levelData.Save();
        _start.Disable();
        SceneManager.LoadScene("Menu");
    }
}
