using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class AssetReferenceGameEvent : AssetReferenceT<GameEvent>
{
    public AssetReferenceGameEvent(string guid) : base(guid) { }
}

public abstract class GameEvent : ScriptableObject
{
    public string eventDescription;
    private static GameObject room1;
    private static GameObject room2;
    protected GameObject room;
    private static bool roomsInitialized = false;

    protected virtual void InitializeRooms()
    {
        if (!roomsInitialized)
        {
            room1 = GameObject.Find("Room1");
            room2 = GameObject.Find("Room2");

            roomsInitialized = true;
        }
    }

    public void Trigger(bool isRoom2)
    {
        Debug.Log("we are triggering, is it room2?" + isRoom2);
        InitializeRooms();

        room = (!isRoom2) ? room1 : room2;
        Execute();
    }

    public void Kill()
    {
        ResetEvent();
    }

    protected abstract void Execute();
    protected abstract void ResetEvent();
    protected GameObject[] ObjectsByTag(string objectTag)
    {
        if (room == null)
        {
            Debug.LogWarning("Room is not assigned.");
            return new GameObject[0];
        }

        GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(objectTag);
        List<GameObject> filteredObjects = new List<GameObject>();

        foreach (GameObject obj in foundObjects)
        {
            if (obj.transform.IsChildOf(room.transform))
            {
                filteredObjects.Add(obj);
            }
        }

        return filteredObjects.ToArray();
    }

}

