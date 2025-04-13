using UnityEngine;

public class FasterAnimationEvent : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animation>()["mixamo.com"].speed = 2f;    
    }
}
