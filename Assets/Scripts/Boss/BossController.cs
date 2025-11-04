using UnityEngine;

public class BossController : MonoBehaviour {
    public static BossController Instance;

    private bool _bossFight;
    private IBoss _boss;

    //Arena
    private Vector3 _startArena;
    private Vector3 _endArena;
    private int _arenaHeight;
    private int _arenaWidth;

    private void Start() {
        Instance = this;
        FindBoss();
        if (_boss != null) FindArenaBlocks();
    }

    private void Update() {
        if (!_bossFight && _boss != null) {
            PlayerInArena();
        } else if (_bossFight) {
            _boss.Fight();
        }
    }

    public bool IsBossFight() {
        return _bossFight;
    }

    public void SharpAttack() {
        _boss.SharpAttack();
    }

    public Vector3 GetStartArena() {
        return _startArena;
    }

    public Vector3 GetEndArena() {
        return _endArena;
    }

    private void FindArenaBlocks() {
        Vector3 start;
        Vector3 end;

        GameObject[] arena = GameObject.FindGameObjectsWithTag("BossArena");
        if (arena != null) {
            bool isBiggest = arena[0].transform.position.y > arena[1].transform.position.y;
            start = isBiggest ? arena[0].transform.position : arena[1].transform.position;
            end = isBiggest ? arena[1].transform.position : arena[0].transform.position;

            _startArena = start;
            _endArena = end;
            _arenaHeight = Mathf.Abs((int)(start.y - end.y)) + 1;
            _arenaWidth = Mathf.Abs((int)(start.x - end.x)) + 1;

        } else {
            Debug.LogError("No arena blocks found");
        }
    }

    private void FindBoss() {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null){
            _boss = boss.GetComponent<IBoss>();
            if (_boss == null) {
                Debug.LogError("Boss does not implement IBoss interface");
            }
        }
    }

    private void PlayerInArena() {
        Vector3 playerPos = Player.Instance.transform.position;

        if (playerPos.x >= _startArena.x && playerPos.y <= _startArena.y) {
            if (playerPos.x <= _endArena.x && playerPos.y >= _endArena.y) {
                _bossFight = true;
                EventController.Instance.EventTriggerActivated(0);
            }
        }
    }
}
