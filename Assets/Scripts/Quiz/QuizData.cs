using System;
using System.Collections.Generic;

[Serializable]
public class QuizData
{
    public Guid ID;
    public QuestionType QuestionType;
    public string Question;
    public string CustomImageID;
    public List<QuizAnswer> Answers;
    public int CorrectAnswerIndex;
}

[Serializable]
public class QuizAnswer
{
    public string ImageID;
    public string Text;
}

public enum QuestionType { Text, Flag }
