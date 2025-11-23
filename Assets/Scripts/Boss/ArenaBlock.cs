using UnityEngine;

public class ArenaBlock : MonoBehaviour
{
    private void Start() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
