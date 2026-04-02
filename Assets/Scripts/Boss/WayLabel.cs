using TMPro;
using UnityEngine;

public class WayLabel : MonoBehaviour
{
    [SerializeField] private int wayNum;
    [SerializeField] private TextMeshProUGUI wayNumText;

    public int WayNum => wayNum;

    private void OnValidate()
    {
        wayNumText.text = wayNum.ToString();
    }

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
    }
}
