using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "QuizEvent", menuName = "ScriptableObjects/Tile Events/Quiz Event")]
public class QuizEvent : TileEvent
{
    public string sceneName;

    public override void Execute(Player player, Action onEventComplete)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
