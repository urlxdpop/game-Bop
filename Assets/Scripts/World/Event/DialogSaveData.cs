using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class DialogSaveData
{
    public List<SceneDialogData> scenes = new();
}

[Serializable]
public class SceneDialogData
{
    public string sceneName;
    public List<EventDialogData> events = new();
}

[Serializable]
public class EventDialogData
{
    public int numberEvent;
    public Dialog dialog;
}


public static class DialogsDataSaver {
    private static string SavePath => Path.Combine(Application.persistentDataPath, "dialogs.json");

    public static void Save(DialogSaveData data) {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static DialogSaveData Load() {
        if (!File.Exists(SavePath))
            return new DialogSaveData();

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<DialogSaveData>(json);
    }
}