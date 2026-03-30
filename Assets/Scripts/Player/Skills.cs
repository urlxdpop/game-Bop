using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Skills : MonoBehaviour
{
    [SerializeField] private LayerMask _foreground;
    [SerializeField] private InputAction _magneticAction;
    [SerializeField] private InputAction _destroyAction;

    private bool _isActive;
    private Vector3 _animPos;

    private Animator _animator;

    private const string DESTROY = "Destroy";
    private const string MAGNET = "Magnet";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SkillsActivated();

        if (_isActive)
        {
            transform.position = _animPos;
        }
    }

    public void CloseAnim()
    {
        _isActive = false;
    }

    public void Destroy()
    {
        _animator.SetTrigger(DESTROY);
        _isActive = true;
        _animPos = Player.Instance.CurrentPos();
        Player.Instance.CheckForDestroy();
    }

    public void Magnet()
    {
        _animator.SetTrigger(MAGNET);
        _isActive = true;
        _animPos = Player.Instance.CurrentPos();
        Player.Instance.CheckForMagnet();
    }

    private void SkillsActivated()
    {
        if (!_isActive)
        {
            if (_destroyAction.triggered)
            {
                Destroy();
            } else if (_magneticAction.triggered)
            {
                Magnet();
            }
        }
    }


    private void OnEnable()
    {
        _magneticAction.Enable();
        _destroyAction.Enable();
    }

    private void OnDisable()
    {
        _magneticAction?.Disable();
        _destroyAction?.Disable();
    }
}
