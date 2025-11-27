using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Text _time;

    private void Update() {
        float time = GameController.Instance.GameTime();
        _time.text = Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart() {
        Player.Instance.Die();
    }

    public void Continue() {
        GameController.Instance.ResumeGame();
    }
}
