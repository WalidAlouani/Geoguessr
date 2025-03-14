using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class RandomResourceKeyPicker : MonoBehaviour
{
    [Tooltip("Label for the JSON files in Addressables.")]
    public AssetLabelReference jsonLabel;

    /// <summary>
    /// Asynchronously gets a random resource key from the Addressables group identified by jsonLabel.
    /// </summary>
    public async Task<string> GetRandomResourceKeyAsync()
    {
        // Load all resource locations with the given label.
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

        // Pick a random location and return its PrimaryKey.
        int randomIndex = Random.Range(0, locations.Count);
        string resourceKey = locations[randomIndex].PrimaryKey;
        Addressables.Release(locationsHandle);
        return resourceKey;
    }

    // For testing, you can call this in Start.
    async void Start()
    {
        string randomKey = await GetRandomResourceKeyAsync();
        Debug.Log("Random resource key: " + randomKey);
    }
}
