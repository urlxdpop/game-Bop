using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScene : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;

    private void Start() {
        SceneManager.LoadScene(_levelData.levelName);
    }
}
