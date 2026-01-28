using SaveData;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DBPlayerController : MonoBehaviour
{
    private const string URL = "http://localhost/DBForGapGame/";

    [Serializable]
    public class ServerError
    {
        public string error;
    }

    [Serializable]
    public class LoginResponse
    {
        public bool success;
        public int userId;
        public string error;
    }

    [Serializable]
    public class CreateUserResponse
    {
        public string error;
        public int userId;
    }

    [Serializable]
    public class UserDataResponse
    {
        public string levelsOpened;
        public string minTimeForLevel;
        public string timeForAllLevels;
        public string isSecretOpen;
        public string numSecretInLevel;
    }

    public void CreatePlayer(string name, string password, GameData levelData, SaveData.SecretsData secretsData, Action<bool, string> callback)
    {
        StartCoroutine(CreatePlayerRequest(name, password, levelData, secretsData, callback));
    }

    public void ValidateLogin(string name, string password, Action<bool, int, string> callback)
    {
        StartCoroutine(LoginRequest(name, password, callback));
    }

    public void UploadData(int userId, GameData levelData, SaveData.SecretsData secretsData, Action<bool, string> callback)
    {
        StartCoroutine(UpdateDataRequest(userId, levelData, secretsData, callback));
    }

    public void LoadUserData(int id, Action<bool, GameData, SaveData.SecretsData> callback)
    {
        StartCoroutine(GetUserDataRequest(id, callback));
    }

    private IEnumerator CreatePlayerRequest(string name, string password, GameData levelData, SaveData.SecretsData secretsData, Action<bool, string> callback)
    {
        string levelOpenedStr = BoolMatrixToString(levelData.levelOpened);
        string minTimeForLevelStr = IntMatrixToString(levelData.minTimeForLevel);
        string secretsOpenedStr = StringMatrixToString(secretsData.isSecretOpen);
        string numSecretsInLevelStr = IntMatrixToString(secretsData.numSecretsInLevel);

        WWWForm form = new();
        form.AddField("name", name);
        form.AddField("password", password);
        form.AddField("levelsOpened", levelOpenedStr);
        form.AddField("minTimeForLevel", minTimeForLevelStr);
        form.AddField("timeForAllLevels", levelData.timeForAllLevels);
        form.AddField("isSecretOpen", secretsOpenedStr);
        form.AddField("numSecretInLevel", numSecretsInLevelStr);

        using UnityWebRequest request = UnityWebRequest.Post(URL + "createUser.php", form);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback(false, "network_error");
            yield break;
        }

        string raw = request.downloadHandler.text;

        if (string.IsNullOrEmpty(raw) || !raw.TrimStart().StartsWith("{"))
        {
            callback(false, "invalid_json");
            yield break;
        }

        CreateUserResponse json = JsonUtility.FromJson<CreateUserResponse>(raw);

        if (!string.IsNullOrEmpty(json.error))
        {
            callback(false, json.error);
            yield break;
        }

        callback(true, "ok");
    }

    private IEnumerator LoginRequest(string name, string password, Action<bool, int, string> callback)
    {
        WWWForm form = new();
        form.AddField("name", name);
        form.AddField("password", password);

        using UnityWebRequest request = UnityWebRequest.Post(URL + "login.php", form);
        yield return request.SendWebRequest();

        string raw = request.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback(false, -1, "network_error");
            yield break;
        }

        if (string.IsNullOrEmpty(raw) || !raw.TrimStart().StartsWith("{"))
        {
            callback(false, -1, "invalid_json");
            yield break;
        }

        LoginResponse json;
        try
        {
            json = JsonUtility.FromJson<LoginResponse>(raw);
        } catch
        {
            callback(false, -1, "json_parse_error");
            yield break;
        }

        if (!string.IsNullOrEmpty(json.error))
        {
            callback(false, -1, json.error);
            yield break;
        }

        callback(true, json.userId, null);
    }

    private IEnumerator UpdateDataRequest(int id, GameData levelData, SaveData.SecretsData secretsData, System.Action<bool, string> callback)
    {

        string levelOpenedStr = BoolMatrixToString(levelData.levelOpened);
        string minTimeForLevelStr = IntMatrixToString(levelData.minTimeForLevel);
        string secretsOpenedStr = StringMatrixToString(secretsData.isSecretOpen);
        string numSecretsInLevelStr = IntMatrixToString(secretsData.numSecretsInLevel);

        WWWForm form = new();
        form.AddField("id", id);
        form.AddField("levelsOpened", levelOpenedStr);
        form.AddField("minTimeForLevel", minTimeForLevelStr);
        form.AddField("timeForAllLevels", levelData.timeForAllLevels);
        form.AddField("isSecretOpen", secretsOpenedStr);
        form.AddField("numSecretInLevel", numSecretsInLevelStr);

        using UnityWebRequest request = UnityWebRequest.Post(URL + "updateUserData.php", form);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback(false, "network_error");
        } else
        {
            callback(true, "ok");
        }
    }

    private IEnumerator GetUserDataRequest(int id, Action<bool, SaveData.GameData, SaveData.SecretsData> callback)
    {

        using UnityWebRequest request = UnityWebRequest.Get(URL + "getUserData.php?id=" + id);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback(false, null, null);
            yield break;
        }

        string response = request.downloadHandler.text;

        var json = JsonUtility.FromJson<UserDataResponse>(response);

        SaveData.GameData gameData = new();
        SaveData.SecretsData secData = new();

        gameData.levelOpened = StringToBoolMatrix(json.levelsOpened);
        gameData.minTimeForLevel = StringToIntMatrix(json.minTimeForLevel);
        gameData.timeForAllLevels = int.Parse(json.timeForAllLevels);
        secData.isSecretOpen = json.isSecretOpen.Split('$');
        secData.numSecretsInLevel = StringToIntMatrix(json.numSecretInLevel);

        callback(true, gameData, secData);
    }

    private string BoolMatrixToString(bool[] matrix)
    {
        string result = "";

        foreach (bool b in matrix)
        {
            result += (b ? "1" : "0") + "$";
        }

        return result;
    }

    private string IntMatrixToString(int[] matrix)
    {
        string result = "";

        foreach (int i in matrix)
        {
            result += i.ToString();
            result += "$";
        }

        return result;
    }

    private string StringMatrixToString(string[] matrix)
    {
        string result = "";

        foreach (string b in matrix)
        {
            result += b + "$";
        }

        return result;
    }

    private bool[] StringToBoolMatrix(string str)
    {
        string[] strArray = str.Split('$', StringSplitOptions.RemoveEmptyEntries);
        bool[] result = new bool[strArray.Length];

        int j = 0;
        foreach (string i in strArray)
        {
            result[j] = strArray[j++] == "1";
        }

        return result;
    }

    private int[] StringToIntMatrix(string str)
    {
        string[] strArray = str.Split('$', StringSplitOptions.RemoveEmptyEntries);
        int[] result = new int[strArray.Length];

        for (int j = 0; j < strArray.Length; j++)
        {
            result[j] = int.Parse(strArray[j]);
        }

        return result;
    }

}
