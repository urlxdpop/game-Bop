using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LaserGunVisual : MonoBehaviour
{
    [SerializeField] private LaserGunController _laserGunController;

    private Animator _animator;

    private const string IS_DIE = "IsDie";
    private const string ON = "On";

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        _animator.SetBool(ON, _laserGunController.HaveLaser());
    }

    public void DestroyObj() {
        _laserGunController.DestroyYourself();
        Destroy(gameObject);
    }

    public void Laser_OnDie() {
        _animator.SetBool(IS_DIE, true);
    }
}
