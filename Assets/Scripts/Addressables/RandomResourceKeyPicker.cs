using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class RandomResourceKeyPicker
{
    public static async UniTask<string> GetRandomResourceKeyAsync(AssetLabelReference jsonLabel)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(jsonLabel);
        await locationsHandle.ToUniTask();

        if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load resource locations for label: " + jsonLabel);
            return null;
        }

        IList<IResourceLocation> locations = locationsHandle.Result;
        if (locations.Count == 0)
        {
            Debug.LogWarning("No resources found for label: " + jsonLabel);
            Addressables.Release(locationsHandle);
            return null;
        }

        int randomIndex = Random.Range(0, locations.Count);
        string resourceKey = locations[randomIndex].PrimaryKey;
        Addressables.Release(locationsHandle);
        return resourceKey;
    }

    public static async UniTask<List<string>> GetRandomResourceKeyAsync(AssetLabelReference jsonLabel, int count)
    {
        List<string> resources = new List<string>();
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(jsonLabel);
        await locationsHandle.ToUniTask();

        if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load resource locations for label: " + jsonLabel);
            return null;
        }

        IList<IResourceLocation> locations = locationsHandle.Result;
        if (locations.Count == 0)
        {
            Debug.LogWarning("No resources found for label: " + jsonLabel);
            Addressables.Release(locationsHandle);
            return null;
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, locations.Count);
            string resourceKey = locations[randomIndex].PrimaryKey;
            locations.RemoveAt(randomIndex); // avoid duplicates
            resources.Add(resourceKey);
        }

        Addressables.Release(locationsHandle);

        return resources;
    }
}
