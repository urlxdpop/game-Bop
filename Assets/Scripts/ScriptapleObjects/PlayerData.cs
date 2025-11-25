using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public int playerId;
    public string playerName;
    public int saveId = -1;

    private const string SAVE_KEY = "PlayerData";

    public void Load() {
        SaveData.PlayerData data = SaveManager.Load<SaveData.PlayerData>(SAVE_KEY);

        playerId = data.playerId;
        playerName = data.playerName;
    }

    public void Save() {
        SaveManager.Save(SAVE_KEY, ToGameData());
    }

    public void Logout() {
        saveId = playerId;
        playerId = -1;
        playerName = "";
        Save();
    }

    private SaveData.PlayerData ToGameData() {
        SaveData.PlayerData data = new() {
            playerId = playerId,
            playerName = playerName,
            saveId = saveId
        };
        return data;
    }
}
