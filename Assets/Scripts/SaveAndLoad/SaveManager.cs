using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string GetPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key + ".json");
    }

    public static void Save<T>(string key, T data)
    {
        string json = JsonUtility.ToJson(data);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        } else if (Application.isMobilePlatform)
        {
            File.WriteAllText(GetPath(key), json);
        } else
        {
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }

    public static T Load<T>(string key)
    {
        string json;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (!PlayerPrefs.HasKey(key))
                return System.Activator.CreateInstance<T>();

            json = PlayerPrefs.GetString(key);
        } else if (Application.isMobilePlatform)
        {
            string path = GetPath(key);

            if (!File.Exists(path))
                return System.Activator.CreateInstance<T>();

            json = File.ReadAllText(path);
        } else
        {
            if (!PlayerPrefs.HasKey(key))
                return System.Activator.CreateInstance<T>();

            json = PlayerPrefs.GetString(key);
        }

        T obj = JsonUtility.FromJson<T>(json);

        obj ??= System.Activator.CreateInstance<T>();

        return obj;
    }
}