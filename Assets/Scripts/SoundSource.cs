using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSource : MonoBehaviour, IPoolable<SoundSource>
{
    [SerializeField] private AudioSource _audioSource;
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

    public void Play(AudioClip clip, AudioMixerGroup mixer)
    {
        if (_audioSource.isPlaying)
        {
            if (_preCoroutine != null)
            {
                Managers.Instance.Coroutine.StopCoroutine(_preCoroutine);
            }

            _audioSource.Stop();
        }

        _audioSource.outputAudioMixerGroup = mixer;
        _audioSource.PlayOneShot(clip);

        Managers.Instance.Coroutine.StartCoroutine(CoReleaseAfterPlaying(clip.length));
    }

    public void Play(AudioClip clip, AudioMixerGroup mixer, Vector3 pos)
    {
        transform.position = pos;
        Play(clip, mixer);
    }

    private IEnumerator CoReleaseAfterPlaying(float clipLength)
    {
        yield return Managers.Instance.Coroutine.GetWaitForSeconds(clipLength);
        Relase();
    }

    private void Relase()
    {
        _audioSource.Stop();
        returnAction?.Invoke(this);
    }
}
