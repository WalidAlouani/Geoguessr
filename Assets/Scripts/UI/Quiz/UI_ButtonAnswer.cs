using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonAnswer : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private UI_ButtonAnswerAnimation anim;

    [SerializeField] private Sprite correct;
    [SerializeField] private Sprite wrong;

    public void Initialize(string answer)
    {
        text.text = answer;
        anim.ShowUp();
    }

    public void SetResponse(bool isCorrect)
    {
        if (isCorrect)
        {
            buttonImage.sprite = correct;
            anim.Shake();
        }
        else
        {
            buttonImage.sprite = wrong;
        }
    }

    public void EnableClick(bool enable)
    {
        button.interactable = enable;
    }
}
