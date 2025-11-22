using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    private Animator _animator;

    private const string MOVE_X = "MoveX";
    private const string MOVE_Y = "MoveY";
    private const string IS_MOVING = "IsMoving";
    private const string MOVING_BOX = "MovingBox";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        Player.Instance.OnRotate += Player_OnRotate;
    }

    private void Update() {
        _animator.SetBool(IS_MOVING, Player.Instance.IsMoving());
        _animator.SetBool(MOVING_BOX, Player.Instance.MovingBox());
    }

    private void Player_OnRotate(object sender, System.EventArgs e) {
        Vector2 diraction = Player.Instance.InputVector();

        _animator.SetFloat(MOVE_X, diraction.x);
        _animator.SetFloat(MOVE_Y, diraction.y);

    }
}
