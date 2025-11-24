using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
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
