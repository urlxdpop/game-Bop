using TMPro;
using UnityEngine;

public class PortalController : MonoBehaviour {
    [SerializeField] private int _numberEvent;
    [SerializeField] private TextMeshProUGUI _textNumEvent;

    private PortalController _secondPortal;

    private void OnValidate() {
        _textNumEvent.text = _numberEvent.ToString();

        FindSecondPortal();
    }

    private void Start() {
        GetComponentInChildren<Canvas>().enabled = false;

        FindSecondPortal();
    }

    public void SetSecondPortal(PortalController portal) {
        _secondPortal = portal;
    }
    
    public Vector3 Teleported() {
        return _secondPortal.transform.position;
    }

    private void FindSecondPortal() {
        PortalController[] portals = FindObjectsByType<PortalController>(FindObjectsSortMode.None);

        foreach (PortalController portal in portals) {
            if (portal != this && portal._numberEvent == _numberEvent) {
                _secondPortal = portal;
                portal.SetSecondPortal(this);
                break;
            }
        }
    }
}
