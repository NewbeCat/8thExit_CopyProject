using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceGameEvent : AssetReferenceT<GameEvent>
{
    public AssetReferenceGameEvent(string guid) : base(guid) { }
}

public abstract class GameEvent : ScriptableObject
{
    public string eventDescription;

    private bool isActive = false;

    public void Trigger()
    {
        if (!isActive)
        {
            isActive = true;
            Execute();
        }
    }

    public void Kill()
    {
        if (isActive)
        {
            ResetEvent();
            isActive = false;
        }
    }
    protected abstract void Execute();
    protected abstract void ResetEvent();
}
