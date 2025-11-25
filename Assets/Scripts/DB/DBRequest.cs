using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DBRequest : MonoBehaviour {

    [SerializeField] private Text _text;
    [SerializeField] private Text _inputName;
    [SerializeField] private Text _inputPassword;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private LevelData _levelData;
    [SerializeField] private SecretsData _secretsData;

    private const string GAME_SAVE_KEY = "LevelsData";
    private const string SECRET_SAVE_KEY = "SecretsData";
    private const string PLAYER_SAVE_KEY = "PlayerData";

    private DBController DB;

    private void Awake() {
        DB = gameObject.AddComponent<DBController>();
    }

    public void Register() {
        CreateUser(_inputName.text, _inputPassword.text);
    }

    public void SignIn() {
        Login(_inputName.text, _inputPassword.text);
    }

    public void SaveDataToDB() {
        SaveData.PlayerData data = SaveManager.Load<SaveData.PlayerData>(PLAYER_SAVE_KEY);
        int userId = data.playerId;
        if (userId == -1) {
            return;
        }
        UploadGameData(userId);
    }

    public void LoadDataFromDB() {
        SaveData.PlayerData data = SaveManager.Load<SaveData.PlayerData>(PLAYER_SAVE_KEY);
        int userId = data.playerId;
        if (userId == -1) {
            return;
        }
        LoadData(userId);
    }

    private void CreateUser(string name, string password) {
        _text.text = "Регистрация...";

        SaveData.GameData levelData = LevelsToGameData();
        SaveData.SecretsData secretsData = SecretsToGameData();

        DB.CreatePlayer(name, password, levelData, secretsData, (success, msg) => {
            _text.text = msg;
            if (!success) {
                if (msg == "name_taken")
                    _text.text = "Имя уже занято!";
                else
                    _text.text = "Ошибка регистрации!";

                return;
            }

            _text.text = "Профиль создан!";

            SignIn();
        });
    }

    private SaveData.GameData LevelsToGameData() {
        SaveData.GameData data = new() {
            levelOpened = _levelData.levelOpened,
            sceneName = _levelData.levelName,
            timeForAllLevels = _levelData.timeForAllLevels,
            minTimeForLevel = _levelData.minTimeForLevel
        };
        return data;
    }

    private SaveData.SecretsData SecretsToGameData() {
        SaveData.SecretsData data = new() {
            isSecretOpen = MatrixInString(_secretsData.isSecretOpen),
            numSecretsInLevel = _secretsData.numSecretsInLevel,
            savingData = _secretsData.savingData
        };
        return data;
    }


    private void Login(string name, string password) {
        _text.text = "Вход...";

        DB.ValidateLogin(name, password, (success, userId, err) => {
            if (!success) {
                if (err == "wrong_password")
                    _text.text = "Неверный пароль!";
                else if (err == "user_not_found")
                    _text.text = "Игрок не найден!";
                else
                    _text.text = "Ошибка входа!";

                return;
            }

            _text.text = "Успешный вход!";

            _playerData.playerId = userId;
            _playerData.playerName = name;
            _playerData.Save();

            LoadData(userId);
        });
    }

    private void UploadGameData(int userId) {
        SaveData.GameData levelData = LevelsToGameData();
        SaveData.SecretsData secretsData = SecretsToGameData();

        DB.UploadData(userId, levelData, secretsData, (success, msg) => {

            if (!success) {
                return;
            }
        });
    }



    private void LoadData(int userId) {
        DB.LoadUserData(userId, (success, gameData, secretData) => {
            if (!success) {
                return;
            }


            SaveManager.Save(GAME_SAVE_KEY, gameData);
            SaveManager.Save(SECRET_SAVE_KEY, secretData);

            if (TryGetComponent(out MainMenu m)){
                m.UpdatePlayerData();
            };


            SceneManager.LoadScene("Menu");
        });
    }

    private string[] MatrixInString(bool[,] matrix) {
        string[] result = new string[matrix.GetLength(0)];
        for (int i = 0; i < matrix.GetLength(0); i++) {
            string row = "";
            for (int j = 0; j < matrix.GetLength(1); j++) {
                row += matrix[i, j] ? "1" : "0";
            }
            result[i] = row;
        }
        return result;
    }
}
