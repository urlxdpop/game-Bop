using DG.Tweening;
using UnityEngine;

enum TypeAttack {
    JUMP,
    SPAWN_WEB,
    SPAWN_SPIDER
}

[SelectionBase]
public class SpiderBossController : MonoBehaviour, IBoss {
    [SerializeField] private float _dalayWithAttack;
    [SerializeField] private float _animationTimer;
    [SerializeField] private int _numWebs = 15;
    [SerializeField] private int _numSpider = 10;
    [SerializeField] private GameObject _warningBlock;
    [SerializeField] private GameObject _webBlock;
    [SerializeField] private GameObject _spider;
    [SerializeField] private GameObject _redSpider;

    private int _hp = 5;
    private bool _isDead;
    private bool _isAttack;
    private bool _isAnimation;
    private float _timer;
    private TypeAttack _typeAttack = TypeAttack.JUMP;
    private Vector3 _attackPos;
    private Vector3[] _SpawnPos;
    private GameObject[] _warningBlocks;
    private float _angle = 0;

    private Collider2D _collider2D;

    private void Start() {
        _collider2D = GetComponent<Collider2D>();

        _warningBlocks = new GameObject[100];
        _SpawnPos = new Vector3[_numWebs];

        ReturnSettings();
    }

    public void Fight() {
        if (_isDead) return;
        _timer += Time.deltaTime;
        RotateToPlayer();
        if (_isAttack) {
            Attack();
        } else if (_timer > 0) {
            _isAttack = true;
            if (_typeAttack == TypeAttack.JUMP) ShowWarningBlockForJump();
            else if (_typeAttack == TypeAttack.SPAWN_WEB) ShowWarningBlockForSpawnWeb();
            else if (_typeAttack == TypeAttack.SPAWN_SPIDER) ShowWarningBlockForSpawnSpider();
        }
    }

    public void SharpAttack() {
        _isAttack = false;
        _typeAttack = TypeAttack.JUMP;
        _timer = 0;
    }

    public void TakeDamage() {
        _hp--;
        if (_hp <= 0) {
            Die();
        }
    }

    public int GetHP() {
        return _hp;
    }

    public void DestroyYourself() {
        EventController.Instance.EventTriggerActivated(1);
        Destroy(gameObject);
    }

    private void Attack() {
        switch (_typeAttack) {
            case TypeAttack.JUMP:
                Jump();
                break;
            case TypeAttack.SPAWN_WEB:
                SpawnWeb();
                break;
            case TypeAttack.SPAWN_SPIDER:
                SpawnSpider();
                break;
            default:
                Debug.LogError("No type attack");
                break;
        }
    }

    private void Jump() {
        if (_timer >= _dalayWithAttack) {
            if (!_isAnimation) {
                _isAnimation = true;
                _collider2D.enabled = false;

                transform.DOMove(_attackPos, _animationTimer);
                transform.DOScale(1.5f, _animationTimer / 2).OnComplete(() => {
                    transform.DOScale(1f, _animationTimer / 2).OnComplete(() => {
                        ReturnSettings();
                        _collider2D.enabled = true;
                    });
                });
            }
        }
    }

    private void SpawnWeb() {
        if (_timer >= _dalayWithAttack) {
            if (!_isAnimation) {
                _isAnimation = true;
                for (int i = 0; i < _SpawnPos.Length; i++) {
                    GameObject web = Instantiate(_webBlock, transform.position, Quaternion.AngleAxis(0, Vector3.forward));
                    web.GetComponent<WebController>().BossCreated(_SpawnPos[i]);
                }
                ReturnSettings();
            }
        }
    }

    private void SpawnSpider() {
        if (_timer >= _dalayWithAttack) {
            if (!_isAnimation) {
                _isAnimation = true;
                for (int i = 0; i < _numSpider; i++) {
                    GameObject spider = Instantiate(Random.Range(0, 1f) > 0.07 ? _spider : _redSpider, transform.position, Quaternion.AngleAxis(0, Vector3.forward));
                    spider.GetComponent<SpiderController>().BossCreated(_SpawnPos[i]);
                    spider.GetComponent<SpiderController>().SetMovingOrientation(GetRandomRotation());
                }
                ReturnSettings();
            }
        }
    }

    private void ShowWarningBlockForJump() {
        _attackPos = Player.Instance.CurrentPos();
        Vector3 startArena = BossController.Instance.GetStartArena();
        Vector3 endArena = BossController.Instance.GetEndArena();

        if (_attackPos.x - 1 < startArena.x) _attackPos.x++;
        else if (_attackPos.x + 1 > endArena.x) _attackPos.x--;
        if (_attackPos.y - 1 < endArena.y) _attackPos.y++;
        else if (_attackPos.y + 1 > startArena.y) _attackPos.y--;

        for (int i = 0; i < 9; i++) {
            _warningBlocks[i] = Instantiate(_warningBlock, new Vector3(_attackPos.x + (i % 3 - 1), _attackPos.y + (i / 3 - 1), _attackPos.z), Quaternion.AngleAxis(0, Vector3.forward));
        }
    }

    private void ShowWarningBlockForSpawnWeb() {
        _attackPos = Player.Instance.CurrentPos();
        Vector3 startArena = BossController.Instance.GetStartArena();
        Vector3 endArena = BossController.Instance.GetEndArena();

        for (int i = 0; i < _numWebs; i++) {
            while (true) {
                int x = (int)Random.Range(_attackPos.x - 4, _attackPos.x + 4);
                int y = (int)Random.Range(_attackPos.y - 4, _attackPos.y + 4);
                Vector3 pos = new(x, y, 0);

                if (pos.x < startArena.x || pos.x > endArena.x || pos.y < endArena.y || pos.y > startArena.y) continue;

                _SpawnPos[i] = pos;
                _warningBlocks[i] = Instantiate(_warningBlock, pos, Quaternion.AngleAxis(0, Vector3.forward));

                break;
            }
        }
    }

    private void ShowWarningBlockForSpawnSpider() {
        Vector3 startArena = BossController.Instance.GetStartArena();
        Vector3 endArena = BossController.Instance.GetEndArena();

        for (int i = 0; i < _numSpider; i++) {
            while (true) {
                int x = (int)Random.Range(startArena.x - 1, endArena.x + 1);
                int y = (int)Random.Range(startArena.y - 1, endArena.y + 1);
                Vector3 pos = new(x, y, 0);

                _SpawnPos[i] = pos;

                _warningBlocks[i] = Instantiate(_warningBlock, pos, Quaternion.AngleAxis(0, Vector3.forward));

                break;
            }
        }
    }

    private void ReturnSettings() {
        _isAnimation = false;
        _isAttack = false;
        _timer = Random.Range(-2f, 0);

        HideWarningBlock();

        switch (Random.Range(0, 4)) {
            case 0:
            case 3:
                _typeAttack = TypeAttack.JUMP; break;
            case 1:
                _typeAttack = TypeAttack.SPAWN_WEB; break;
            case 2:
                _typeAttack = TypeAttack.SPAWN_SPIDER; break;
        }
    }

    private void HideWarningBlock() {
        for (int i = 0; i < _warningBlocks.Length; i++) {
            if (_warningBlocks[i] != null) {
                Destroy(_warningBlocks[i]);
                _warningBlocks[i] = null;
            }
        }
    }

    private void RotateToPlayer() {
        Vector3 dir = Player.Instance.transform.position - transform.position;
        if (_typeAttack != TypeAttack.JUMP) dir = -dir;
        _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 270;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }

    private Vector3 GetRandomRotation() {
        return Random.Range(0, 4) switch {
            0 => new(-1, 0),
            1 => new(0, 1),
            2 => new(0, -1),
            _ => new(1, 0)
        };
    }

    private void Die() {
        _collider2D.enabled = false;
        _isDead = true;

        GetComponentInChildren<SpiderBossVisual>().Die();
    }
}
