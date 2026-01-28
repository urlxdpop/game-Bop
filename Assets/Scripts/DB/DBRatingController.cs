using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

class DBRatingController : MonoBehaviour
{
    private const string URL = "http://localhost/DBForGapGame/";

    [Serializable]
    public class RatingEntry
    {
        public int id;
        public string name;
        public int timeForAllLevels;
        public string levelsOpened ;
    }

    [Serializable]
    public class RatingList
    {
        public RatingEntry[] rating;
    }

    public void GetRating(Action<bool, RatingEntry[]> callback)
    {
        StartCoroutine(GetRatingRequest(callback));
    }

    private IEnumerator GetRatingRequest(Action<bool, RatingEntry[]> callback)
    {
        using UnityWebRequest request = UnityWebRequest.Get(URL + "GetRating.php");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            callback(false, null);
            Debug.LogError("Error while sending request: " + request.error);
            yield break;
        }

        string response = request.downloadHandler.text;

        if (string.IsNullOrEmpty(response) || !response.TrimStart().StartsWith("["))
        {
            callback(false, null);
            Debug.LogError("Invalid response from server: " + response);
            yield break;
        }

        RatingEntry[] rating;
        try
        {
            rating = JsonHelper.FromJson<RatingEntry>(response);
        }
        catch
        {
            callback(false, null);
            Debug.LogError("Error parsing JSON response: " + response);
            yield break;
        }

        callback(true, rating);
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}