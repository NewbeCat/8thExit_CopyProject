using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour, IManager
{
    private Dictionary<float, WaitForSeconds> _waitForSecondDict = new Dictionary<float, WaitForSeconds>();

    public void Init()
    {
        
    }

    public WaitForSeconds GetWaitForSeconds(float duration)
    {
        if (_waitForSecondDict.TryGetValue(duration, out WaitForSeconds waitForSeconds))
        {
            return waitForSeconds;
        }

        _waitForSecondDict.Add(duration, new WaitForSeconds(duration));
        return _waitForSecondDict[duration];
    }
}
