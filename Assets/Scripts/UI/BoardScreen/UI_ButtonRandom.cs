using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TurnManager turnManager;//remove later
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    private bool isInside = false;

    private WaitForSeconds wait = new WaitForSeconds(0.02f);

    //subscribe to game state

    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ThrowDice());
    }

    private IEnumerator ThrowDice()
    {
        //button.interactable = false;
        text.gameObject.SetActive(true);

        for (int i = 0; i < 10; i++)
        {
            text.text = i.ToString();
            yield return wait;
        }

        var steps = turnManager.RollDice();

        text.text = steps.ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isInside)
            return;

        image.DOFade(1, 0.15f);
        text.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.DOFade(0, 0.15f);
    }
}
