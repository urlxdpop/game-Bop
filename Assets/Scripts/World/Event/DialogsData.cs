using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogsData", menuName = "ScriptableObjects/DialogsData", order = 1)]
public class DialogsData : ScriptableObject {
    // sceneName => numberEvent => Dialog
    public Dictionary<string, Dictionary<int, Dialog>> dialogs = new Dictionary<string, Dictionary<int, Dialog>>();

    public void AddDialog(string sceneName, int numberEvent, Dialog dialog) {
        if (dialog == null) {
            return;
        }

        if (!dialogs.ContainsKey(sceneName)) {
            dialogs[sceneName] = new Dictionary<int, Dialog>();
        }

        if (!dialogs[sceneName].ContainsKey(numberEvent)) {
            dialogs[sceneName][numberEvent] = dialog;
            AssetDatabase.SaveAssets();
            return;
        }

        if (dialogs[sceneName][numberEvent] != dialog) {
            dialogs[sceneName][numberEvent] = dialog;
            AssetDatabase.SaveAssets();
        }
    }

    public Dialog GetDialog(string sceneName, int numberEvent) {
        if (dialogs.TryGetValue(sceneName, out var eventDict)) {
            if (eventDict.TryGetValue(numberEvent, out var dialog)) {
                return dialog;
            }
        }
        return null;
    }
}