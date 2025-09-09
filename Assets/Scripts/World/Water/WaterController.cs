using UnityEngine;

public class WaterController : MonoBehaviour, IEvent
{
    public void Interact() {
        Player.Instance.InWater();
    }    
}
