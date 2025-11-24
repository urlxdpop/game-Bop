using SaveData;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SecretsData", menuName = "ScriptableObjects/SecretsData", order = 1)]
public class SecretsData : ScriptableObject {
    public bool[,] isSecretOpen = new bool[100, 10];
    public int[] numSecretsInLevel = new int[100];
    public bool[] savingData = new bool[100];

    private const string SAVE_KEY = "SecretsData";

    public void Load() {
        SaveData.SecretsData data = SaveManager.Load<SaveData.SecretsData>(SAVE_KEY);

        isSecretOpen = StringInMatrix(data.isSecretOpen);
        numSecretsInLevel = data.numSecretsInLevel;
        savingData = data.savingData;
    }

    public void Save() {
        SaveManager.Save(SAVE_KEY, ToGameData());
    }

    public void SecretFound(string levelName, int numSecret) {
        int[] level = levelName.Split('-').Select(x => int.Parse(x)).ToArray();
        int levelIndex = (level[0] - 1) * 15 + (level[1] - 1);

        isSecretOpen[levelIndex, numSecret] = true;

        Save();
    }

    public void SaveNumSecretInLevel(string levelName, int numSecrets) {
        int[] level = levelName.Split('-').Select(x => int.Parse(x)).ToArray();
        int levelIndex = (level[0] - 1) * 15 + (level[1] - 1);
        if (savingData[levelIndex]) return;

        numSecretsInLevel[levelIndex] = numSecrets;
        savingData[levelIndex] = true;

        Save();
    }

    public void ResetData() {
        numSecretsInLevel = new int[100];
        savingData = new bool[100];
        isSecretOpen = new bool[100, 10];

        Save();
    }

    private SaveData.SecretsData ToGameData() {
        SaveData.SecretsData data = new() {
            isSecretOpen = MatrixInString(isSecretOpen),
            numSecretsInLevel = numSecretsInLevel,
            savingData = savingData
        };
        return data;
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

    private bool[,] StringInMatrix(string[] strings) {
        bool[,] result = new bool[strings.Length, strings[0].Length];
        for (int i = 0; i < strings.Length; i++) {
            for (int j = 0; j < strings[i].Length; j++) {
                result[i, j] = strings[i][j] == '1';
            }
        }
        return result;
    }
}
