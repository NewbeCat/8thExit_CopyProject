using UnityEngine;

[CreateAssetMenu(menuName = "GameEvents/PosterEvent")]
public class PosterEvent : GameEvent
{
    public Transform poster;
    private Vector3 originalSize;
    private Vector3 shrunkSize;

    protected override void Execute()
    {
        originalSize = poster.localScale;
        shrunkSize = originalSize * 0.8f;
        poster.localScale = shrunkSize;
        Debug.Log("Poster shrank!");
    }

    protected override void ResetEvent()
    {
        poster.localScale = originalSize;
        Debug.Log("Poster returned to normal size.");
    }
}
