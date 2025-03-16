using System;
using System.Threading.Tasks;
using UnityEngine;

public class QuizFactory
{
    public static async Task<QuizDataBase<TAnswer>> CreateQuizAsync<TAnswer>(QuizData quizData, IAssetLoader assetLoader, ICountryCodeLookup countryCodeLookup)
    {
        switch (quizData.QuestionType)
        {
            case QuizType.Text:
                if (typeof(TAnswer) != typeof(string))
                    throw new ArgumentException("For a text quiz, the generic type TAnswer must be string.");

                var textQuiz = await TextQuiz.CreateAsync(quizData, assetLoader);
                return textQuiz as QuizDataBase<TAnswer>;

            case QuizType.Flag:
                if (typeof(TAnswer) != typeof(Sprite))
                    throw new ArgumentException("For a flag quiz, the generic type TAnswer must be Sprite.");
                var flagQuiz = await FlagQuiz.CreateAsync(quizData, assetLoader, countryCodeLookup);
                return flagQuiz as QuizDataBase<TAnswer>;

            default:
                throw new NotSupportedException($"Quiz type {quizData.QuestionType} is not supported.");
        }
    }
}
