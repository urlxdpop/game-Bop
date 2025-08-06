using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

[SelectionBase]
public class EventBlockController : MonoBehaviour, IBlockEvent
{
    [SerializeField] private int _numberEvent;
    [SerializeField] private TextMeshProUGUI _textNumEvent;
    [SerializeField] private Tilemap _forground;
    [SerializeField] private Tile _wallBlock;

    private bool _isHaveBlock;
    private Vector3Int _roundPos;

    private void OnValidate() {
        _textNumEvent.text = _numberEvent.ToString();
    }

    private void Start() {
        transform.Find("Visual").gameObject.SetActive(false);

        _roundPos = Vector3Int.RoundToInt(transform.position);
        _roundPos -= new Vector3Int(1,1,0);

        if (_forground == null) {
            _forground = GameObject.Find("Forground").GetComponent<Tilemap>();
        }

        _isHaveBlock = IsHaveBlock();
    }

    public int NumberEvent() {
        return _numberEvent;
    }

    public void Activated() {
        SetOrResetBlock(true);
    }

    public void Deactivated() {
        SetOrResetBlock(false);
    }

    private bool IsHaveBlock() {
        if (_forground.GetTile(_roundPos)) return true;
        return false;
    }

    private void SetOrResetBlock(bool isActivated) {
        if (isActivated ? _isHaveBlock : !_isHaveBlock) {
            _forground.SetTile(_roundPos, null);
        } else {
            _forground.SetTile(_roundPos, _wallBlock);
        }
    }
}
