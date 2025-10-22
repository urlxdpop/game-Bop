using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageVisual : MonoBehaviour
{
    private Image _damageImage;

    private void Awake()
    {
        _damageImage = GetComponent<Image>();
        _damageImage.enabled = false;
    }

    public void TakeDamage() {
        _damageImage.enabled = true;
        _damageImage.DOColor(new Color(1f,0,0,0), 1f).OnComplete(() => {
            _damageImage.enabled = false;
            _damageImage.color = new Color(1f, 0, 0, 0.5f);
        });
    }
}
