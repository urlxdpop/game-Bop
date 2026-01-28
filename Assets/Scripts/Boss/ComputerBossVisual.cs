using UnityEngine;

public class ComputerBossVisual : MonoBehaviour
{
    private Animator _animator;
    private ComputerBossController _computerBossController;

    private const string FAZE = "Faze";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _computerBossController = GetComponentInParent<ComputerBossController>();
    }

    private void Update()
    {
        _animator.SetInteger(FAZE, _computerBossController.Faze());
    }
}
