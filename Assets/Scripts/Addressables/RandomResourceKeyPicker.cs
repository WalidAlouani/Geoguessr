using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class RandomResourceKeyPicker
{
    public static async Task<string> GetRandomResourceKeyAsync(AssetLabelReference jsonLabel)
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(jsonLabel);
        await locationsHandle.Task;

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
}
