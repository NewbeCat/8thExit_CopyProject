using System.Collections.Generic;
using UnityEngine;

public class HallPoster : MonoBehaviour
{
    [SerializeField] private List<Sprite> imageAssets;
    [SerializeField] private List<GameObject> posterObjects;
    private int posterNum = 0;
    public void UpdatePosters(int index)
    {
        int indexi = 0;
        if (index > 0) indexi = index;

        if (posterNum != indexi)
        {
            foreach (GameObject obj in posterObjects)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();

                if (renderer == null)
                {
                    Debug.LogWarning($"Missing renderer on {obj.name}");
                    continue;
                }

                renderer.sprite = imageAssets[indexi];
            }
        }
    }
}