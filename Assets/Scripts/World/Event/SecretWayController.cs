
using UnityEngine;

public class SecretWayController : MonoBehaviour, IEvent
{
    [SerializeField] private SecretsData _secretsData;

    private int _numberSecret = 0;

    private void Start() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void Interact() {
        SecretsController.Instance.SecretFound(this);
    }

    public void SetNumberSecret(int number) {
        _numberSecret = number;
    }

    public int GetNumberSecret() {
        return _numberSecret;
    }
}
