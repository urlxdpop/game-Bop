using System.Runtime.CompilerServices;
using UnityEngine;

public class SpiderBossVisual : MonoBehaviour {
    [SerializeField] private GameObject _boom;
    [SerializeField] private float _time = 1f;

    private SpriteRenderer _spriteRenderer;

    private bool _isDie = false;
    private float _timer = 0;

    private float _timerSpawnBoom = 0;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (_isDie) {
            _timer += Time.deltaTime;
            if (_timer < _time) {
                SpawnBoom();
            } else {
                Debug.Log("die"); 
                DestroyYourself();
            }
        }
    }

    public void Die() {
        _isDie = true;
        Debug.Log("kill");
    }

    private void SpawnBoom() {
        _timerSpawnBoom += Time.deltaTime;
        if (_timerSpawnBoom > 0.1f) {
            Vector3 pos = new(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + Random.Range(-1.5f, 1.5f));
            Instantiate(_boom, pos, Quaternion.AngleAxis(0, Vector3.forward));
            _timerSpawnBoom = 0;
        }
    }

    private void DestroyYourself() {
        _spriteRenderer.enabled = false;
        GetComponentInParent<SpiderBossController>().DestroyYourself();
    }
}
