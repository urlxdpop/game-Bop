using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }

    private PlayerInputAction _playerInputAction;

    private void Awake() {
        Instance = this;

        _playerInputAction = new PlayerInputAction();
        _playerInputAction.Enable();
    }

    public Vector2 GetMovementAction() {
        Vector2 inputVector = _playerInputAction.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }
}
