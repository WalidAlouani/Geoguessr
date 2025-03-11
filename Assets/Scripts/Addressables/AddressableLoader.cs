using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class AddressableLoader: IAssetLoader
{
    private readonly Dictionary<string, AsyncOperationHandle> loadedAssetHandles = new Dictionary<string, AsyncOperationHandle>();

    /// <summary>
    /// Loads an asset from the Addressables system asynchronously by its address (name or ID).
    /// Implements caching: If the asset is already loaded, it returns the cached instance.
    /// </summary>
    /// <typeparam name="T">The type of asset to load (e.g., Texture2D, Sprite, GameObject).</typeparam>
    /// <param name="assetId">The address (name or ID) of the asset in Addressables.</param>
    /// <returns>The loaded asset of type T, or null if loading fails. The operation is wrapped in a Task for async handling.</returns>
    public async Task<T> LoadAssetAsync<T>(string assetId) where T : Object
    {
        if (loadedAssetHandles.TryGetValue(assetId, out AsyncOperationHandle existingHandle))
        {
            if (existingHandle.IsValid() && existingHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[AddressableLoader] Returning cached asset '{assetId}' of type '{typeof(T).Name}'.");
                return existingHandle.Result as T; // Return cached result
            }
            else
            {
                Debug.LogWarning($"[AddressableLoader] Cached handle for '{assetId}' is invalid or not succeeded. Re-loading.");
                ReleaseAsset(assetId); // Release invalid cache entry before reloading
            }
        }

        Debug.Log($"[AddressableLoader] Loading asset '{assetId}' of type '{typeof(T).Name}' from Addressables.");
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetId);
        await handle.Task; // Wait for the asset to load

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            loadedAssetHandles[assetId] = handle; // Cache the handle
            return handle.Result;
        }
        else
        {
            Debug.LogError($"[AddressableLoader] Failed to load Addressable asset '{assetId}' of type '{typeof(T).Name}'. Error: {handle.OperationException?.Message}");
            Addressables.Release(handle); // Release handle on failure as well for cleanup
            return null; // Or throw an exception based on your error handling strategy
        }
    }

    /// <summary>
    /// Releases a loaded Addressable asset by its name/ID, freeing up resources.
    /// It retrieves the handle from the internal cache using the asset name.
    /// </summary>
    /// <param name="assetId">The address (name or ID) of the asset to release.</param>
    public void ReleaseAsset(string assetId)
    {
        if (loadedAssetHandles.TryGetValue(assetId, out AsyncOperationHandle handle))
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
                Debug.Log($"[AddressableLoader] Released Addressable asset '{assetId}'.");
            }
            else
            {
                Debug.LogWarning($"[AddressableLoader] Attempted to release asset '{assetId}', but the handle was already invalid.");
            }
            loadedAssetHandles.Remove(assetId); // Remove from cache after release
        }
        else
        {
            Debug.LogWarning($"[AddressableLoader] Attempted to release asset '{assetId}', but it was not found in the loaded assets cache. Was it loaded through this loader?");
        }
    }

    public void ReleaseAll()
    {
        foreach (var item in loadedAssetHandles.Keys.ToList())
        {
            ReleaseAsset(item);
        }

        loadedAssetHandles.Clear();
    }
}