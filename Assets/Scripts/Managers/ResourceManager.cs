using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ResourceManager : IManager
{

    public void Init()
    {
    }

    public void LoadAddressableAssetFromLabel<T>(EAddressableLabel label, Action<AsyncOperationHandle<T>> handle)
    {
        Addressables.LoadResourceLocationsAsync(label.ToString(), typeof(T)).Completed += (locationHandle) => OnLocationLoaded(locationHandle, handle);
    }

    private void OnLocationLoaded<T>(AsyncOperationHandle<IList<IResourceLocation>> locationsHandle, Action<AsyncOperationHandle<T>> handle)
    {
        if (locationsHandle.Status == AsyncOperationStatus.Succeeded)
        {
            if (locationsHandle.Result.Count == 0)
            {
                // you can call completion callback
                Debug.Log("Location is Loaded");
                return;
            }

            foreach (var location in locationsHandle.Result)
            {
                Addressables.LoadAssetAsync<T>(location).Completed += handle;
            }
        }
        else
        {
            Debug.Assert(false, "AsyncOperationStatus is Failed");
        }
    }
}
