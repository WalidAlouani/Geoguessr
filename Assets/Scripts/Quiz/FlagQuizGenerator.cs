using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FlagQuizGenerator
{
    private AssetLabelReference assetLabel;
    private ICountryCodeLookup countryCodeLookup;
    private string questionTemplate = "Which flag belongs to {0}?";

    public FlagQuizGenerator(ICountryCodeLookup countryCodeLookup, AssetLabelReference assetLabel)
    {
        this.countryCodeLookup = countryCodeLookup;
        this.assetLabel = assetLabel;
    }

    public async UniTask<QuizData> Generate()
    {
        var res = await RandomResourceKeyPicker.GetRandomResourceKeyAsync(assetLabel, 4);

        var correctIndex = Random.Range(0, res.Count);

        string correctAnswer = countryCodeLookup.GetCountryName(res[correctIndex]);

        var question = string.Format(questionTemplate, correctAnswer);

        var answers = new List<QuizAnswer>();

        for (int i = 0; i < res.Count; i++)
        {
            answers.Add(new QuizAnswer() { ImageID = res[i] });
        }

        return new QuizData()
        {
            ID = new System.Guid(),
            Question = question,
            QuestionType = QuizType.Flag,
            Answers = answers,
            CorrectAnswerIndex = correctIndex,
        };
    }
}
