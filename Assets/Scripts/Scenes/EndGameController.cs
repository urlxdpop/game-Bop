using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public void NextGame() {
        string sceneName = SceneManager.GetActiveScene().name;

        string[] splitScene = sceneName.Split('-');
        int sceneHead = int.Parse(splitScene[0]);
        int sceneAct = int.Parse(splitScene[1]);

        sceneAct++;
        string sceneToLoad = sceneHead + "-" + sceneAct;
        Scene nextSceneName = SceneManager.GetSceneByName(sceneToLoad);
        if (nextSceneName.IsValid()) {
            SceneManager.LoadScene(sceneToLoad);
        } else {
            sceneHead++;
            sceneAct = 1;
            sceneToLoad = sceneHead + "-" + sceneAct;
            nextSceneName = SceneManager.GetSceneByName(sceneToLoad);
            if (nextSceneName.IsValid()) {
                SceneManager.LoadScene(sceneToLoad);
            } else {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
