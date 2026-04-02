using UnityEngine;

public class PlayerBossVisual : MonoBehaviour
{
    private Animator _animator;
    private PlayerBoss _playerBoss;

    private const string IS_MOVING = "IsMoving";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerBoss = GetComponentInParent<PlayerBoss>();  
    }

    private void Start()
    {
        _playerBoss.OnFight += PlayerBoss_OnFight;
        _playerBoss.OnCaught += PlayerBoss_OnCaught;
    }

    private void Update()
    {
        Vector3 dir = _playerBoss.GetDir();

        _animator.SetFloat("MoveX", dir.x);
        _animator.SetFloat("MoveY", dir.y);
    }

    private void PlayerBoss_OnFight(object sender, System.EventArgs e)
    {
        _animator.SetBool(IS_MOVING, true);
    }

    private void PlayerBoss_OnCaught(object sender, System.EventArgs e)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _animator.SetBool(IS_MOVING, false);
    }

    private void OnDestroy()
    {
        _playerBoss.OnFight -= PlayerBoss_OnFight;
        _playerBoss.OnCaught -= PlayerBoss_OnCaught;
    }
}
