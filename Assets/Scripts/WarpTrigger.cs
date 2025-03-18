using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    [SerializeField] private WarpZone _warpZone;
    [SerializeField] private int _entryType;
    [SerializeField] private bool _entryGroup;

    public void Initialize(WarpZone zone, int type, bool group)
    {
        _warpZone = zone;
        _entryType = type;
        _entryGroup = group;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _warpZone.WarpPlayer(_entryType, _entryGroup);
        }
    }
}
