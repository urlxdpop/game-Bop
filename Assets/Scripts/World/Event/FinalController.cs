using UnityEngine;

public class FinalController : MonoBehaviour, IEvent
{

    private void Awake() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void Interact() {
        GameController.Instance.TheEndGame();
    }
}
