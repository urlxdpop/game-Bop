using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start() {
        Instantiate(_player, transform.position, transform.rotation);
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
