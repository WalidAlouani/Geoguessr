using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "QuizEvent", menuName = "ScriptableObjects/Tile Events/Quiz Event")]
public class QuizEvent : TileEvent
{
    public QuizType quizType;
    public string sceneName;

    public override void Execute(TileItem tile, Player player, SignalBus signalBus)
    {
        signalBus.Fire(new QuizRequestedSignal(player, tile, quizType));
    }
}
