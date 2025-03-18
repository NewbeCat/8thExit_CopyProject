using UnityEngine;

public class SoundManager : MonoBehaviour, IManager
{
    [SerializeField] private SoundSource _soundSourcePrefab;
    [SerializeField] private int _poolInitCount = 20;
    [SerializeField] private int _poolMaxCount = 50;
    private ObjectPooler<SoundSource> _pooler;

    public void Init()
    {
        _pooler = new ObjectPooler<SoundSource>(_soundSourcePrefab, transform, _poolInitCount, _poolMaxCount);    
    }

    public void Play(/*EAudioClipType*/)
    {
        SoundSource soundSource = _pooler.Pool();
        // Clip ����
        // ���
        // AssetBundle�̳� AddressableAsset ���
        //soundSource.Play();
    }
}
