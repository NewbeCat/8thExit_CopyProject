using System;
using System.Collections;
using UnityEngine;

public class SoundSource : MonoBehaviour, IPoolable<SoundSource>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ESoundType _eSoundType = ESoundType.SFX;
    
    // Return To PoolManager
    public event Action<SoundSource> returnAction;

    private Coroutine _preCoroutine;

    public void Play(AudioClip clip)
    {
        if (_audioSource.isPlaying)
        {
            if (_preCoroutine != null)
            {
                Managers.Instance.Coroutine.StopCoroutine(_preCoroutine);
            }

            _audioSource.Stop();
        }

        _audioSource.PlayOneShot(clip);

        Managers.Instance.Coroutine.StartCoroutine(CoReleaseAfterPlaying(clip.length));
    }

    public void Play(AudioClip clip, Vector3 pos)
    {
        transform.position = pos;
        Play(clip);
    }

    private IEnumerator CoReleaseAfterPlaying(float clipLength)
    {
        yield return Managers.Instance.Coroutine.GetWaitForSeconds(clipLength);
        _audioSource.Stop();
    }
}
