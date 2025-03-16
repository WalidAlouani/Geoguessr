public class QuizRequestedSignal
{
    public Player Player { get; private set; }
    public TileItem Tile { get; private set; }
    public QuizType QuizType { get; private set; }

    public QuizRequestedSignal(Player player, TileItem tile, QuizType quizType)
    {
        Player = player;
        Tile = tile;
        QuizType = quizType;
    }
}
