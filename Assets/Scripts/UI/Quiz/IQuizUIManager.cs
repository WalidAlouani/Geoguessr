public interface IQuizUIManager<TQuiz, TData>
    where TQuiz : QuizDataBase<TData>
{
    void Initialize(QuizManagerBase<TQuiz, TData> quizManager, TQuiz quiz);
}



