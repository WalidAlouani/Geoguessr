using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonAnswerFlag : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private UI_ButtonAnswerAnimation anim;

    [SerializeField] private GameObject correct;
    [SerializeField] private GameObject wrong;

    public void Initialize(Sprite answer)
    {
        image.sprite = answer;
        anim.ShowUp();
    }

    public void SetResponse(bool isCorrect, bool withAnimation = true)
    {
        if (isCorrect)
        {
            correct.SetActive(true);
            if (withAnimation)
                anim.Shake();
        }
        else
        {
            wrong.SetActive(true);
        }
    }

    public void EnableClick(bool enable)
    {
        button.interactable = enable;
    }
}
