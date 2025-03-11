using System.Threading.Tasks;
using UnityEngine;

public interface IAssetLoader
{
    Task<T> LoadAssetAsync<T>(string assetId) where T : Object;
    void ReleaseAsset(string assetId);
    void ReleaseAll();
}