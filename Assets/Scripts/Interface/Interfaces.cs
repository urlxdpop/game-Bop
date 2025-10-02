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

public interface IImpulseObject {
    public void Impulse(Vector3 dir, float speed);
}

public interface IBoss {
    public void Fight();
    public void SharpAttack();
}
