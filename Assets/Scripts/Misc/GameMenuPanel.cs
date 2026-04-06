using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _resume;
    [SerializeField] private TextMeshProUGUI _goToMenu;
    [SerializeField] private TextMeshProUGUI _restart;

    bool _isRus;

    private void Start()
    {
        _isRus = GameController.Instance.IsRus;
        if (_isRus)
        {
            _title.text = "Меню";
            _resume.text = "Продолжить";
            _goToMenu.text = "В главное меню";
            _restart.text = "Начать заново";
        }else
        {
            _title.text = "Menu";
            _resume.text = "Resume";
            _goToMenu.text = "Main menu";
            _restart.text = "Restart";
        }
    }
}
