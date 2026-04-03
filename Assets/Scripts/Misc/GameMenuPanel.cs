using UnityEngine;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _resume;
    [SerializeField] private Text _goToMenu;
    [SerializeField] private Text _restart;

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
        }
    }
}
