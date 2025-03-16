using System.Collections;
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

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        this.levelName = sceneName;
        StartCoroutine(LoadLevelAsync(loadSceneMode));
    }

    public void UnloadScene(string sceneName)
    {
        this.levelName = sceneName;
        StartCoroutine(UnloadLevelAsync());
    }

    IEnumerator LoadLevelAsync(LoadSceneMode loadSceneMode)
    {
        loadingScreen.SetActive(true);
        
        loadingScreen.GetComponent<Image>()
            .DOFade(1, fadeInDuration);

        float timer = 0f;
        while (timer < fakeProgressDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName, loadSceneMode);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 1f);
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName)); // Make the new scene active
        
        loadingScreen
            .GetComponent<Image>()
            .DOFade(0, fadeOutDuration)
            .OnComplete(()=> loadingScreen.SetActive(false));
    }

    IEnumerator UnloadLevelAsync()
    {
        loadingScreen.SetActive(true);

        loadingScreen.GetComponent<Image>()
            .DOFade(1, fadeInDuration);

        float timer = 0f;
        while (timer < fakeProgressDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        AsyncOperation operation = SceneManager.UnloadSceneAsync(levelName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 1f);
            yield return null;
        }

        loadingScreen
            .GetComponent<Image>()
            .DOFade(0, fadeOutDuration)
            .OnComplete(() => loadingScreen.SetActive(false));
    }
}
