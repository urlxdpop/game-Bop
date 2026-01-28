using UnityEngine;
using UnityEngine.UI;

public class PlayerRecord : MonoBehaviour
{
    [SerializeField] private Text _number;
    [SerializeField] private Text _name;
    [SerializeField] private Text _time;

    public void SetRecord(int number, string name, string time)
    {
        _number.text = number.ToString();
        _name.text = name;
        _time.text = time;
    }

    public void ClearRecord()
    {
        _number.text = "-";
        _name.text = "...";
        _time.text = "...";
    }
}
