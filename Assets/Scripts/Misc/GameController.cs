using UnityEngine;
using UnityEngine.Tilemaps;

public enum GameState {
    FREE_ROAM,
    DIALOG,
    END_GAME,
}

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    [SerializeField] private Tilemap _foregroundTilemap;
    [SerializeField] private GameObject _endGame;

    private GameState _gameState;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _gameState = GameState.FREE_ROAM;

        SubscribeForActionEvent();

        SetCameraLimits();
    }

    private void Update() {
        HandlerState();
    }

    public void TheEndGame() {
        _gameState = GameState.END_GAME;
    }

    private void SetCameraLimits() {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        _foregroundTilemap.CompressBounds();
        cameraController.SetLimit(
            _foregroundTilemap.localBounds.max.x,
            _foregroundTilemap.localBounds.min.x,
            _foregroundTilemap.localBounds.min.y,
            _foregroundTilemap.localBounds.max.y
        );
    }

    private void SubscribeForActionEvent() {
        DialogManager.Instance.OnShowDialog += () => {
            _gameState = GameState.DIALOG;
        };
        DialogManager.Instance.OnHideDialog += () => {
            if (_gameState == GameState.DIALOG) _gameState = GameState.FREE_ROAM;
        };
    }

    private void HandlerState() {
        switch (_gameState) {
            case GameState.FREE_ROAM:
                Player.Instance.HandlerUpdate();
                break;
            case GameState.DIALOG:
                DialogManager.Instance.HandleUpdate();
                break;
            case GameState.END_GAME:
                _endGame.SetActive(true);
                break;
        }
    }
}
