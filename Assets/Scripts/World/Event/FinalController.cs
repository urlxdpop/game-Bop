using UnityEngine;

public class FinalController : MonoBehaviour, IEvent
{
    public void Interact() {
        GameController.Instance.TheEndGame();
    }
}
