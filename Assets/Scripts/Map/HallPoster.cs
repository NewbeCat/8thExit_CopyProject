using System.Collections.Generic;
using UnityEngine;

public class HallPoster : MonoBehaviour
{
    [SerializeField] private List<GameObject> posterObjects;
    [SerializeField] private List<string> imagePaths; // Resources 내 경로들

    private List<Sprite> imageAssets = new List<Sprite>();

    private void Start()
    {
        Cursor.visible = false;
        foreach (string path in imagePaths) imageAssets.Add(Resources.Load<Sprite>(path));
    }


    public void UpdatePosters(int index)
    {
        Debug.Log("Currently room " + index);
        foreach (GameObject obj in posterObjects)
        {
            SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
            renderer.sprite = imageAssets[index];
        }
    }
}