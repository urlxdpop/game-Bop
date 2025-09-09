using UnityEngine;

public class DeathBlockController : MonoBehaviour, IEvent {
    private void Start() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void Interact() {
        Player.Instance.Die();
    }
}
