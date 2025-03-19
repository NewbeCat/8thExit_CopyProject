using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour, IManager
{
    private Dictionary<ESoundClip, AudioClip> _audioClipDict = new Dictionary<ESoundClip, AudioClip>();

    [SerializeField] private SoundSource _soundSourcePrefab;
    [SerializeField] private int _poolInitCount = 20;
    [SerializeField] private int _poolMaxCount = 50;
    private ObjectPooler<SoundSource> _pooler;

    public void Init()
    {
        _pooler = new ObjectPooler<SoundSource>(_soundSourcePrefab, transform, _poolInitCount, _poolMaxCount);
        Managers.Instance.Resource.LoadAddressableAssetFromLabel<AudioClip>(EAddressableLabel.Sound, OnSoundAssetLoaded);
    }

    public void Play(/*EAudioClipType*/)
    {
        SoundSource soundSource = _pooler.Pool();
        // Clip 정보
        // 경로
        // AssetBundle이나 AddressableAsset 사용
        //soundSource.Play();
    }

    private void OnSoundAssetLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (Enum.TryParse(handle.Result.name, out ESoundClip eSoundClip))
        {
            _audioClipDict.TryAdd(eSoundClip, handle.Result);
        }
        else
        {
            Debug.Assert(false, "ESoundClip Doesn't match in Addressable");
        }
    }
}
