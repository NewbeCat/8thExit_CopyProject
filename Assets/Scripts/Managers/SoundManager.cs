using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour, IManager
{
    private Dictionary<ESoundClip, AudioClip> _audioClipDict = new Dictionary<ESoundClip, AudioClip>();
    private Dictionary<ESoundClip, List<AudioClip>> _ambienceAudioClipDict = new Dictionary<ESoundClip, List<AudioClip>>();

    [SerializeField] private SoundSource _soundSourcePrefab;
    [SerializeField] private int _poolInitCount = 20;
    [SerializeField] private int _poolMaxCount = 50;
    private ObjectPooler<SoundSource> _pooler;

    public void Init()
    {
        _pooler = new ObjectPooler<SoundSource>(_soundSourcePrefab, transform, _poolInitCount, _poolMaxCount);
        Managers.Instance.Resource.LoadAddressableAssetFromLabel<AudioClip>(EAddressableLabel.FootStep, OnSoundAssetLoaded);
    }

    /// <summary>
    /// Play SoundClip
    public void Play(ESoundClip eSoundClip)
    {
        SoundSource soundSource = _pooler.Pool();
        AudioClip audioClip = GetAmbienceAudioClipRandomly(eSoundClip);
        if (audioClip == null)
        {
            audioClip = GetAudioClip(eSoundClip);
        }

        soundSource.Play(audioClip);
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
        }
    }
}
