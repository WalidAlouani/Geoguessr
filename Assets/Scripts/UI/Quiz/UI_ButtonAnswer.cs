using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonAnswer : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TMP_Text text;

    [SerializeField] private Sprite correct;
    [SerializeField] private Sprite wrong;

    public event Action<int> OnClicked;
    private int index;

    private void OnEnable()
    {
        button.onClick.AddListener(() => OnClicked?.Invoke(index));
    }

    public void Initialize(QuizAnswer answer, int index)
    {
        this.index = index;
        if (answer.Text != string.Empty)
            text.text = answer.Text;

        transform.localScale = Vector3.one * 0.2f;
        transform.DOScale(1, 0.5f).SetEase(Ease.InBack); // change with animation clip
    }

    public void SetResponse(bool isCorrect)
    {
        buttonImage.sprite = isCorrect ? correct : wrong;
    }

    public void EnableClick(bool enable)
    {
        button.interactable = enable;
    }
}
