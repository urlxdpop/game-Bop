using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour {
    [SerializeField] private Text _levelName;
    [SerializeField] private Text _timeInLevel;
    [SerializeField] private Text _minTimeForLevel;
    [SerializeField] private Text _timeInGame;
    [SerializeField] private Text _secretsInLevel;
    [SerializeField] private Text _secretInGame;

    public void SetData(int time, LevelData levelsData, SecretsData secretsData) {
        _levelName.text = SceneManager.GetActiveScene().name;

        string[] splitScene = _levelName.text.Split('-');
        int sceneHead = int.Parse(splitScene[0]);
        int sceneAct = int.Parse(splitScene[1]);
        int index = (sceneHead - 1) * 15 + sceneAct - 1;

        _timeInLevel.text = "Время на уровень: " + Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString();
        _minTimeForLevel.text = "Мин время на уровень: " +
                Mathf.FloorToInt(levelsData.minTimeForLevel[index] / 60).ToString() + ":" +
                Mathf.FloorToInt(levelsData.minTimeForLevel[index] % 60).ToString();
        _timeInGame.text = "Общее время: " +
                Mathf.FloorToInt(levelsData.timeForAllLevels / 60).ToString() + ":" +
                Mathf.FloorToInt(levelsData.timeForAllLevels % 60).ToString();

        int secretsInLevel = 0;
        int secretInGame = 0;
        int allSecrets = 0;
        for (int i = 0; i < secretsData.numSecretsInLevel.Length; i++) {
            for (int j = 0; j < secretsData.numSecretsInLevel[i]; j++) {
                if (secretsData.isSecretOpen[i, j]) {
                    secretInGame++;
                    if (i == index) {
                        secretsInLevel++;
                    }
                }
                allSecrets++;
            }
        }

        _secretsInLevel.text = $"Секреты: {secretsInLevel}/{secretsData.numSecretsInLevel[index]}";
        _secretInGame.text = $"Сереты в общем: {secretInGame}/{allSecrets}";
    }

    public void NextGame() {
        string sceneName = SceneManager.GetActiveScene().name;

        string[] splitScene = sceneName.Split('-');
        int sceneHead = int.Parse(splitScene[0]);
        int sceneAct = int.Parse(splitScene[1]);

        sceneAct++;
        string sceneToLoad = sceneHead + "-" + sceneAct;

        if (Application.CanStreamedLevelBeLoaded(sceneToLoad)) {
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        sceneHead++;
        sceneAct = 1;
        sceneToLoad = sceneHead + "-" + sceneAct;

        if (Application.CanStreamedLevelBeLoaded(sceneToLoad)) {
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        SceneManager.LoadScene("Menu");
    }
}
