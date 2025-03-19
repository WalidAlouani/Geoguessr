public class QuizRequestedSignal
{
    public IPlayer Player { get; private set; }
    public TileItem Tile { get; private set; }
    public QuizType QuizType { get; private set; }
    public string SceneName { get; }

    public QuizRequestedSignal(IPlayer player, TileItem tile, QuizType quizType, string sceneName)
    {
        Player = player;
        Tile = tile;
        QuizType = quizType;
        SceneName = sceneName;
    }
}
