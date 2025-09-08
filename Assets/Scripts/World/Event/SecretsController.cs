using UnityEngine;
using UnityEngine.UI;

public class SecretsController : MonoBehaviour
{
    public static SecretsController Instance { get; private set; }

    [SerializeField] private SecretsData _secretsData;
    [SerializeField] private Text _textNumSecrets;
    [SerializeField] private GameObject _textBox;

    private int _numSecretsInLevel;
    private int _numSecretsFound;
    private int _level;
    private float _timeDisplay = 2f;
    private float _timer = 2f;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        LevelName();
        NumSecrets();

        _textBox.SetActive(false);
    }

    private void Update() {
        ShowDisplay();
    }

    public void SecretFound(SecretWayController secretWay) {
        Vector3 pos = secretWay.transform.position;

        int i = 0;
        for (i = 0; i < _numSecretsInLevel; i++) {
            if (_secretsData.SecretWayPosition[i] == pos) {
                if (!_secretsData.IsSecretOpen[i]) {
                    _numSecretsFound++;
                    _secretsData.IsSecretOpen[i] = true;

                    _textNumSecrets.text = "Secret find: " + _numSecretsFound + " / " + _numSecretsInLevel;
                    _timer = 0;
                }
                break;
            }
        }
    }

    private void NumSecrets() {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int i = 0;
        foreach (var item in allObjects) {
            if (item.GetComponent<SecretWayController>()) {
                _secretsData.SecretWayPosition[i] = item.transform.position;
                _secretsData.NumSecretsInLevel[_level] = i + 1;
                _secretsData.IsSecretOpen[i] = false;
                item.GetComponent<SecretWayController>().SetNumberSecret(i);
                i++;
            }
        }

        _numSecretsInLevel = i;
    }

    private void LevelName() {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        string[] SplitScene = sceneName.Split('-');
        int head = int.Parse(SplitScene[0]) - 1;
        int act = int.Parse(SplitScene[1]) - 1;
        _level = head * 15 + act;
    }

    private void ShowDisplay() {
        if (_timer == 0) {
            _textBox.SetActive(true);
            _timer += Time.deltaTime;
        } else { 
            if (_timer < _timeDisplay) {
                _timer += Time.deltaTime;
            } else if (_timer < _timeDisplay+1){
                _textBox.SetActive(false);
            }
        }
    }
}
