using UnityEngine;

public enum GameState {
    FREE_ROAM,
    DIALOG
}

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }

    private GameState _gameState;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _gameState = GameState.FREE_ROAM;

        SubscribeForActionEvent();
    }

    private void Update() {
        HandlerState();
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
        }
    }
}
