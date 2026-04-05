using UnityEngine;
using YG;

public class MobileController : MonoBehaviour
{
    private void Awake()
    {
        if (!YG2.envir.isMobile) gameObject.SetActive(false);
    }
}
