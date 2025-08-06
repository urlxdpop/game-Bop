using UnityEngine;

public interface IEvent
{
    public void Interact();
}

public interface IBlockEvent {
    public int NumberEvent();
    public void Activated();
    public void Deactivated();
}

public interface IMobs {
    public void DestroyYourself();
}
