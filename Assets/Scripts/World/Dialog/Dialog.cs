using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Dialog
{
    [SerializeField] private List<DialogAutors> _speakers;
    [SerializeField] private List<string> _lines;
    [SerializeField] private List<string> _linesEng;

    public List<string> Lines {
        get { return (GameController.Instance.IsRus ? _lines : _linesEng); }
    }

    internal List<DialogAutors> Speakers {
        get { return _speakers; }
    }

    
}
