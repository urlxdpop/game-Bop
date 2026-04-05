using UnityEngine;

public static class SaveManager
{
    public static void Save<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public static T Load<T>(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            return System.Activator.CreateInstance<T>();

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<T>(json)
               ?? System.Activator.CreateInstance<T>();
    }
}