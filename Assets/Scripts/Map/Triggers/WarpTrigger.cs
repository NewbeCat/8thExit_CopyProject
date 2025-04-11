using UnityEngine;

public class WarpTrigger : MonoBehaviour
{
    [SerializeField] private WarpZone warpZone;
    [SerializeField] private bool _outCorr;
    [SerializeField] private bool active;

    public void Initialize(WarpZone warp, bool outCorr)
    {
        warpZone = warp;
        _outCorr = outCorr;
        active = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.CompareTag("Player"))
        {
            Debug.Log("warp triggering!");
            warpZone.WarpPlayer(_outCorr);
        }
    }

    public void controlTrigger(bool change)
    {
        active = change;
    }
}
