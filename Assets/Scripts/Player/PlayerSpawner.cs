using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Awake() {
        Instantiate(_player, transform.position, transform.rotation);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
