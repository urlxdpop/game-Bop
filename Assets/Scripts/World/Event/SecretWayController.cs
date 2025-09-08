using TMPro;
using UnityEngine;

public class SecretWayController : MonoBehaviour, IEvent
{
    //[SerializeField] private TextMeshProUGUI _textNumEvent;
    [SerializeField] private SecretsData _secretsData;

    private int _numberSecret = 0;

    private void OnValidate() {
        //_textNumEvent.text = _numberSecret.ToString();
    }

    public void Interact() {
        SecretsController.Instance.SecretFound(this);
    }

    public void SetNumberSecret(int number) {
        _numberSecret = number;
    }
}
