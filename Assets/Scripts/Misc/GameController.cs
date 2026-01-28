using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GameState
{
    FREE_ROAM,
    DIALOG,
    END_GAME,
    MENU
}

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
}

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private Tilemap _foregroundTilemap;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private SecretsData _secretData;

    private GameObject _endGame;
    private GameObject _menu;
    private GameState _gameState;
    private CameraController _cameraController;

    private float _gameTime;

    private void Awake()
    {
        Instance = this;

        _endGame = GameObject.Find("EndGame");
        _endGame.SetActive(false);

    }

    private void Start()
    {
        _gameTime = 0;

        _menu = GetComponentInChildren<GameMenu>().gameObject;
        _menu.SetActive(false);

        _gameState = GameState.FREE_ROAM;

        SubscribeForActionEvent();

        SetCameraLimits();
    }

    private void Update()
    {
        if (_cameraController == null)
        {
            SetCameraLimits();
        }

        HandlerState();
    }

    public float GameTime()
    {
        return _gameTime;
    }

    public void TheEndGame()
    {
        _gameState = GameState.END_GAME;
        _levelData.LevelComplate(SceneManager.GetActiveScene().name, (int)_gameTime);
        _endGame.GetComponent<EndGameController>().SetData((int)_gameTime, _levelData, _secretData);
        GetComponent<DBPlayerRequest>().SaveDataToDB();
    }

    public void OpenMenu()
    {
        _menu.SetActive(true);
        _gameState = GameState.MENU;
        Player.Instance.StopMoving();
    }

    public void ResumeGame()
    {
        _menu.SetActive(false);
        _gameState = GameState.FREE_ROAM;
    }

    private void SetCameraLimits()
    {
        if (Camera.main != null && Camera.main.TryGetComponent<CameraController>(out var component))
        {
            _cameraController = component;
            _foregroundTilemap.CompressBounds();
            _cameraController.SetLimit(
                _foregroundTilemap.localBounds.max.x,
                _foregroundTilemap.localBounds.min.x,
                _foregroundTilemap.localBounds.min.y,
                _foregroundTilemap.localBounds.max.y
            );
        }
    }

    private void SubscribeForActionEvent()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            _gameState = GameState.DIALOG;
            Player.Instance.StopMoving();
        };

        DialogManager.Instance.OnHideDialog += () =>
        {
            if (_gameState == GameState.DIALOG) _gameState = GameState.FREE_ROAM;
        };
    }

    private void HandlerState()
    {
        switch (_gameState)
        {
            case GameState.FREE_ROAM:
                Player.Instance.HandlerUpdate();
                _gameTime += Time.deltaTime;
                break;
            case GameState.DIALOG:
                DialogManager.Instance.HandleUpdate();
                _gameTime += Time.deltaTime;
                break;
            case GameState.END_GAME:
                _endGame.SetActive(true);
                break;
            case GameState.MENU:
                break;
        }
    }


}
