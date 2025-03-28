using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour, IManager
{
    private Dictionary<ESoundClip, AudioClip> _audioClipDict = new Dictionary<ESoundClip, AudioClip>();
    private Dictionary<ESoundClip, List<AudioClip>> _ambienceAudioClipDict = new Dictionary<ESoundClip, List<AudioClip>>();

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;

    [SerializeField] private SoundSource _soundSourcePrefab;
    [SerializeField] private int _poolInitCount = 20;
    [SerializeField] private int _poolMaxCount = 50;
    private ObjectPooler<SoundSource> _pooler;

    #region Unity Methods
    #endregion

    public void Init()
    {
        _pooler = new ObjectPooler<SoundSource>(_soundSourcePrefab, transform, _poolInitCount, _poolMaxCount);
        // 로딩창 필요
        Managers.Instance.Resource.LoadAddressableAssetFromLabel<AudioClip>(EAddressableLabel.Ambience, OnSoundAssetLoaded);
        Managers.Instance.Resource.LoadAddressableAssetFromLabel<AudioClip>(EAddressableLabel.FootStep, OnSoundAssetLoaded);
    }

    public void PlayBGM(ESoundClip eSoundClip)
    {
        StopBGM();
        AudioClip audioClip = GetAudioClip(eSoundClip);

        if (audioClip == null)
        {
            Debug.Assert(false, "Audio Clip doesn't exist matches with ESoundClip");
            return;
        }

        _bgmSource.clip = audioClip;
        _bgmSource.Play();
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    // 2D
    public void PlaySFX(ESoundClip eSoundClip)
    {
        SoundSource soundSource = _pooler.Pool();

        if (soundSource == null)
        {
            Debug.Assert(false, "SoundSource Pool is Empty");
            return;
        }

        AudioClip audioClip = TryGetClipOrNull(eSoundClip);
        
        if (audioClip == null)
        {
            Debug.Assert(false, "Audio Clip doesn't exist matches with ESoundClip");
            return;
        }

        soundSource.Play(audioClip, _sfxMixerGroup);
    }

    // 3D
    public void PlaySFX(ESoundClip eSoundClip, Vector3 pos)
    {
        SoundSource soundSource = _pooler.Pool();

        if (soundSource == null)
        {
            Debug.Assert(false, "SoundSource Pool is Empty");
            return;
        }

        AudioClip audioClip = TryGetClipOrNull(eSoundClip);

        if (audioClip == null)
        {
            Debug.Assert(false, "Audio Clip doesn't exist matches with ESoundClip");
            return;
        }

        soundSource.Play(audioClip, _sfxMixerGroup, pos);
    }

    private AudioClip TryGetClipOrNull(ESoundClip eSoundClip)
    {
        AudioClip audioClip = GetAmbienceAudioClipRandomly(eSoundClip);
        if (audioClip == null)
        {
            audioClip = GetAudioClip(eSoundClip);
        }

        return audioClip;
    }

    private AudioClip GetAmbienceAudioClipRandomly(ESoundClip eSoundClip)
    {
        if (_ambienceAudioClipDict.TryGetValue(eSoundClip, out List<AudioClip> clips))
        {
            int index = UnityEngine.Random.Range(0, clips.Count);
            AudioClip clip = clips[index];
            return clip;
        }

        return null;
    }

    private AudioClip GetAudioClip(ESoundClip eSoundClip)
    {
        if (_audioClipDict.TryGetValue(eSoundClip, out AudioClip clip))
        {
            return clip;
        }

        Debug.Assert(false, "dictionary doesn't have key");
        return null;
    }

    // sound file name must split '_'
    // ex) (name)_{number}
    private void OnSoundAssetLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        string[] splits = handle.Result.name.Split('_');
        AudioClip audioClip = handle.Result;

        if (!Enum.TryParse(splits[0], out ESoundClip soundClip))
        {
            Debug.Assert(false, "ESoundClip Doesn't match in Addressable");
            return;
        }

        // ambience
        if (splits.Length > 1)
        {
            if (!_ambienceAudioClipDict.ContainsKey(soundClip))
            {
                _ambienceAudioClipDict.Add(soundClip, new List<AudioClip>());
            }
                
            if (!_ambienceAudioClipDict[soundClip].Contains(audioClip))
            {
                _ambienceAudioClipDict[soundClip].Add(audioClip);
            }
        }
        else
        {
            _audioClipDict.TryAdd(soundClip, audioClip);
            // TODO: 이거 지우고 로딩 창 만들어서 로딩 될 때 PlayBGM 해야함
            if (soundClip == ESoundClip.Ambience)
            {
                PlayBGM(ESoundClip.Ambience);
            }
        }
    }
}
