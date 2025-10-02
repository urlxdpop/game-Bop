using System.Linq;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController Instance { get; private set; }

    private bool[] _activated;
    private IBlockEvent[] events;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _activated = new bool[999];

        events = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IBlockEvent>().ToArray();
    }

    public bool IsActivatedEvent(int numderEvent) {
        return _activated[numderEvent];
    }

    public void Activate(int numberEvent) { 
        _activated[numberEvent] = true;
    }

    public IBlockEvent[] GetAllEvents() {
        return events;
    }

    public void EventTriggerActivated(int numberEvent) {
        foreach (IBlockEvent e in events) {
            if (e.NumberEvent() == numberEvent) {
                e.Activated();
            }
        }
    }

    public void EventTriggerDeactivated(int numberEvent) {
        foreach (IBlockEvent e in events) {
            if (e.NumberEvent() == numberEvent) {
                e.Deactivated();
            }
        }
    }
}
