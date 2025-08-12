using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserGunVisual : MonoBehaviour
{
    [SerializeField] private LaserGunController _laserGunController;

    private Animator _animator;

    private const string IS_DIE = "IsDie";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void DestroyObj() {
        _laserGunController.DestroyYourself();
        Destroy(gameObject);
    }

    public void Laser_OnDie() {
        _animator.SetBool(IS_DIE, true);
    }
}
