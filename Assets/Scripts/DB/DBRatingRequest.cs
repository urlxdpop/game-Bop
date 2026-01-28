using UnityEngine;
using UnityEngine.UI;
using System;

class DBRatingRequest : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private GameObject[] _playerRecords;

    private DBRatingController DB;

    private void Awake()
    {
        DB = gameObject.AddComponent<DBRatingController>();
    }

    private void Start()
    {
        GetRating();
    }

    private void GetRating()
    {
        ClearRating();
        _text.text = "Загрузка рейтинга...";
        DB.GetRating(OnRatingLoaded);
    }

    private void OnRatingLoaded(bool success, DBRatingController.RatingEntry[] rating)
    {
        if (!success)
        {
            _text.text = "Ошибка загрузки";
            _playerRecords = null;
            return;
        }

        if (rating == null || rating.Length == 0)
        {
            _text.text = "Рейтинг пуст";
            return;
        }

        for (int i = 0; i < rating.Length; i++)
        {
            var entry = rating[i];
            _playerRecords[i].GetComponent<PlayerRecord>().SetRecord(i + 1, entry.name, TimeSpan.FromSeconds(entry.timeForAllLevels).ToString(@"hh\:mm\:ss"));
        }

        _text.text = "";
    }

    private void ClearRating()
    {
        PlayerRecord[] records = GetComponentsInChildren<PlayerRecord>();

        foreach (var record in records)
        {
            record.ClearRecord();
        }
    }
}