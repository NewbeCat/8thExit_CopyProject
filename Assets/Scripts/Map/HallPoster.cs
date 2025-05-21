using System.Collections.Generic;
using UnityEngine;

public class HallPoster : MonoBehaviour
{
    private List<Sprite> imageAssets = new List<Sprite>();
    [SerializeField] private List<GameObject> posterObjects;
    private int posterNum = 0;

    private void Start()
    {
        for (int i = 0; i <= 8; i++)
        {
            var sprite = Resources.Load<Sprite>($"hall_poster/hall{i}");
            if (sprite != null)
            {
                imageAssets.Add(sprite);
            }
            else
            {
                Debug.LogWarning($"poster{i} not found");
            }
        }
    }

    public void UpdatePosters(int index)
    {
        Debug.Log("Currently room " + index);
        int indexi = 0;
        if (index > 0) indexi = index;

        if (posterNum != indexi)
        {
            posterNum = indexi;
            foreach (GameObject obj in posterObjects)
            {
                SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();

                if (renderer == null)
                {
                    Debug.LogWarning($"Missing renderer on {obj.name}");
                    continue;
                }
                obj.SetActive(false);
                renderer.sprite = imageAssets[indexi];
                obj.SetActive(true);
            }
        }
    }
}