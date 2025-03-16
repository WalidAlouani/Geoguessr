using UnityEngine.SceneManagement;

public class QuizCommand : ICommand
{
    public QuizType QuizType { get; }
    public string SceneName { get; }

    public QuizCommand(QuizType quizType, string sceneName)
    {
        QuizType = quizType;
        SceneName = sceneName;
    }


    public void Execute()
    {
        SceneLoader.Instance.LoadScene(SceneName, LoadSceneMode.Additive);
    }
}
