using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAssetLoader
{
    UniTask<T> LoadAssetAsync<T>(string assetId) where T : Object;
    void ReleaseAsset(string assetId);
    void ReleaseAll();
}