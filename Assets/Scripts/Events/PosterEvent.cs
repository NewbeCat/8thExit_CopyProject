using UnityEngine;

public class PosterEvent : GameEvent
{
    public Transform poster;
    private Vector3 originalSize;
    private Vector3 shrunkSize;

    void Start()
    {
        originalSize = poster.localScale;

        // 크기 변화를 무작위, 위치 변화, 아예 보이지 않음, 이미지 변화 등을 여러 포스터 중 하나에 부여  // 랜덤부여할지 따로 할지 논의
        shrunkSize = originalSize * 0.8f;
    }

    protected override void Execute()
    {
        poster.localScale = shrunkSize;
        Debug.Log("Poster shrank!");
    }

    protected override void ResetEvent()
    {
        poster.localScale = originalSize;
        Debug.Log("Poster returned to normal size.");
    }
}
