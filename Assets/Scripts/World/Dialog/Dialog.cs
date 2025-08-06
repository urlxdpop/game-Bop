using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Dialog
{
    [SerializeField] private List<string> _lines;

    public List<string> Lines {
        get { return _lines; }
    }
}
