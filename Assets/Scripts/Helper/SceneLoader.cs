using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private float fakeProgressDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    private string levelName;
    private CancellationToken _token;

    private void Start()
    {
        _token = this.GetCancellationTokenOnDestroy();
    }

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        this.levelName = sceneName;
        LoadLevelAsync(loadSceneMode).Forget();
    }

    public void UnloadScene(string sceneName)
    {
        this.levelName = sceneName;
        UnloadLevelAsync().Forget();
    }

    private async UniTaskVoid LoadLevelAsync(LoadSceneMode loadSceneMode)
    {
        loadingScreen.SetActive(true);
        
        loadingScreen.GetComponent<Image>()
            .DOFade(1, fadeInDuration);

        float timer = 0f;
        while (timer < fakeProgressDuration)
        {
            _token.ThrowIfCancellationRequested();
            timer += Time.deltaTime;
            await UniTask.Yield(_token);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName, loadSceneMode);

        while (!operation.isDone)
        {
            _token.ThrowIfCancellationRequested();
            float progress = Mathf.Clamp01(operation.progress / 1f);
            await UniTask.Yield(_token);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName)); // Make the new scene active
        
        loadingScreen
            .GetComponent<Image>()
            .DOFade(0, fadeOutDuration)
            .OnComplete(()=> loadingScreen.SetActive(false));
    }

    private async UniTaskVoid UnloadLevelAsync()
    {
        loadingScreen.SetActive(true);

        loadingScreen.GetComponent<Image>()
            .DOFade(1, fadeInDuration);

        float timer = 0f;
        while (timer < fakeProgressDuration)
        {
            _token.ThrowIfCancellationRequested();
            timer += Time.deltaTime;
            await UniTask.Yield(_token);
        }

        AsyncOperation operation = SceneManager.UnloadSceneAsync(levelName);

        while (!operation.isDone)
        {
            _token.ThrowIfCancellationRequested();
            float progress = Mathf.Clamp01(operation.progress / 1f);
            await UniTask.Yield(_token);
        }

        loadingScreen
            .GetComponent<Image>()
            .DOFade(0, fadeOutDuration)
            .OnComplete(() => loadingScreen.SetActive(false));
    }
}
