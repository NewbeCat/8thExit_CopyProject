using UnityEngine;

public class WarpZone : MonoBehaviour
{
    [SerializeField] private GameObject _goA;
    [SerializeField] private GameObject _goB;
    [SerializeField] private Transform _A;
    [SerializeField] private Transform _B;

    [SerializeField] private GameObject _player;
    private CharacterController _playerController;

    [SerializeField] private bool _lastEntry = true; // false = A, true = B

    private void Start()
    {
        _playerController = _player.GetComponent<CharacterController>();
        _goA.AddComponent<WarpTrigger>().Initialize(this, false);
        _goB.AddComponent<WarpTrigger>().Initialize(this, true);
    }

    public void WarpPlayer(bool currentEntry)
    {
        if (_lastEntry != currentEntry)
        {
            Debug.Log("NO");
        }
        else
        {
            Debug.Log("YES");
        }

        _lastEntry = currentEntry;
        _playerController.enabled = false;
        _player.transform.position += WarpAmount(currentEntry ? _A : _B, currentEntry ? _B : _A);
        _playerController.enabled = true;
    }

    private Vector3 WarpAmount(Transform startLocation, Transform endLocation)
    {
        return new Vector3(
            endLocation.position.x - startLocation.position.x,
            0f,
            endLocation.position.z - startLocation.position.z
        );
    }

}

public class WarpTrigger : MonoBehaviour
{
    private WarpZone _warpZone;
    private bool _isAorB;

    public void Initialize(WarpZone zone, bool isAorB)
    {
        _warpZone = zone;
        _isAorB = isAorB;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _warpZone.WarpPlayer(_isAorB);
        }
    }
}
