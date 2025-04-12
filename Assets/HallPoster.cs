using System.Collections.Generic;
using UnityEngine;

public class HallPoster : MonoBehaviour
{
    [SerializeField] private List<Sprite> imageAssets;
    [SerializeField] private List<GameObject> posterObjects;
    public void UpdatePosters(int index)
    {
        foreach (GameObject obj in posterObjects)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();

            if (renderer == null)
            {
                Debug.LogWarning($"Missing renderer on {obj.name}");
                continue;
            }

            renderer.sprite = imageAssets[index];
        }
    }
}