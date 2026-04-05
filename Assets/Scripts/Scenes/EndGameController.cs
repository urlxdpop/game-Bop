using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour {
    [SerializeField] private Text _levelName;
    [SerializeField] private Text _timeInLevel;
    [SerializeField] private Text _minTimeForLevel;
    [SerializeField] private Text _timeInGame;
    [SerializeField] private Text _secretsInLevel;
    [SerializeField] private Text _secretInGame;
    [SerializeField] private Text _title;
    [SerializeField] private InputAction _OpenMainMenuAction;
    [SerializeField] private InputAction _NextGameAction;
    [SerializeField] private InputAction _RestartAction;

    private bool _isRus;

    private void Start()
    {
        _title.text = _isRus ? "Уровень пройден!" : "Level completed!";
    }

    private void Update()
    {
        if (_OpenMainMenuAction.triggered) OpenMainMenu();
        else if (_NextGameAction.triggered) NextGame();
        else if (_RestartAction.triggered) Restart();
    }

    public void SetData(int time, LevelData levelsData, SecretsData secretsData) {
        _isRus = GameController.Instance.IsRus;
        _levelName.text = SceneManager.GetActiveScene().name;

        string[] splitScene = _levelName.text.Split('-');
        int sceneHead = int.Parse(splitScene[0]);
        int sceneAct = int.Parse(splitScene[1]);
        int index = (sceneHead - 1) * 15 + sceneAct - 1;

        _timeInLevel.text = (_isRus ? "Время на уровень: " : "Time per level: ") + Mathf.FloorToInt(time / 60).ToString() + ":" + Mathf.FloorToInt(time % 60).ToString();
        _minTimeForLevel.text = (_isRus ? "Мин время на уровень: " : "Min time per level: ") +
                Mathf.FloorToInt(levelsData.minTimeForLevel[index] / 60).ToString() + ":" +
                Mathf.FloorToInt(levelsData.minTimeForLevel[index] % 60).ToString();
        _timeInGame.text = (_isRus ? "Общее время: " : "Total time: ") +
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

        _secretsInLevel.text = $"{(_isRus ? "Секреты" : "Secrets")}: {secretsInLevel}/{secretsData.numSecretsInLevel[index]}";
        _secretInGame.text = $"{(_isRus ? "Сереты в общем" : "Total secrets")}: {secretInGame}/{allSecrets}";
    }

    public void NextGame() {
        DOTween.KillAll();
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnEnable()
    {
        _OpenMainMenuAction.Enable();
        _NextGameAction.Enable();
        _RestartAction.Enable();
    }

    private void OnDisable() {
        _OpenMainMenuAction.Disable();
        _NextGameAction.Disable();
        _RestartAction.Disable();
    }
}
