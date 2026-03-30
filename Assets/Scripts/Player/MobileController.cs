using UnityEngine;

public class MobileController : MonoBehaviour
{
    private void Awake()
    {
        if (!Application.isMobilePlatform) gameObject.SetActive(false);
    }
}
