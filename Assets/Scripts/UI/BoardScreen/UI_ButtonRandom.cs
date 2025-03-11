using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ButtonRandom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private GameManager gameManager;//remove later
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;

    private bool isInside = false;

    private WaitForSeconds wait = new WaitForSeconds(0.02f);

    //subscribe to game state

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        isInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        isInside = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        StartCoroutine(ThrowDie());
    }

    private IEnumerator ThrowDie()
    {
        text.gameObject.SetActive(true);

        for (int i = 0; i < 10; i++)
        {
            text.text = i.ToString();
            yield return wait;
        }

        text.text = gameManager.PlayTurn().ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (isInside)
            return;

        image.DOFade(1, 0.15f);
        text.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        image.DOFade(0, 0.15f);
    }
}
