using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ButtonAnswerAnimation : MonoBehaviour
{
    public void ShowUp()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f).SetEase(Ease.InBack); // change with animation clip
    }

    public void Shake()
    {
        transform.DOShakeScale(.3f, .5f, 3, 0);
    }
}
