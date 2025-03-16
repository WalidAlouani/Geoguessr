public abstract class QuizDataBase<TAnswer>
{
    public string Question;
    public int CorrectAnswerIndex;
    public TAnswer[] Answers;

    protected QuizDataBase(QuizData quizData)
    {
        Question = quizData.Question;
        CorrectAnswerIndex = quizData.CorrectAnswerIndex;
    }
}
