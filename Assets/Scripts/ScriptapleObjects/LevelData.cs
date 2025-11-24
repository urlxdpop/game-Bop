using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public bool[] levelOpened;
    public int[] minTimeForLevel;
    public int timeForAllLevels;

    private static string SAVE_KEY => "LevelsData";

    public void Load() {
        SaveData.GameData data = SaveManager.Load<SaveData.GameData>(SAVE_KEY);

        levelName = data.sceneName;
        levelOpened = data.levelOpened;
        minTimeForLevel = data.minTimeForLevel;
        timeForAllLevels = data.timeForAllLevels;
    }

    public void Save() {
        SaveManager.Save(SAVE_KEY, ToGameData());
    }

    public void LevelComplate(string levelName, int time) {
        int[] level = levelName.Split('-').Select(x => int.Parse(x)).ToArray();
        int levelIndex = (level[0] - 1) * 15 + (level[1] - 1);

        levelOpened[levelIndex + 1] = true;
        this.levelName = level[1] < 15 ? $"{level[0]}-{level[1] + 1}" : $"{level[0] + 1}-1";
        minTimeForLevel[levelIndex] = minTimeForLevel[levelIndex] != 0 ? Mathf.Min(minTimeForLevel[levelIndex], time) : time;
        timeForAllLevels += time;

        Save();
    }

    public void SetLevelName() {
        int i = 0;
        while (i < levelOpened.Length && levelOpened[i]) {
            i++;
        }

        int head = Mathf.CeilToInt(i/15);
        int act = (i%15)+1;
        levelName = $"{head}-{act}";
    }

    public void ResetData() {
        levelName = "1-1";
        timeForAllLevels = 0;
        minTimeForLevel = new int[100];
        levelOpened = new bool[100];
        levelOpened[0] = true;
        Save();
    }

    private SaveData.GameData ToGameData() {
        SaveData.GameData data = new() {
            levelOpened = levelOpened,
            sceneName = levelName,
            timeForAllLevels = timeForAllLevels,
            minTimeForLevel = minTimeForLevel
        };
        return data;
    }
}
