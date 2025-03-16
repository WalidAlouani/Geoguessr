using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerTopBar : MonoBehaviour
{
    [SerializeField] private PlayerSetupByTurn playerSetup;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private Outline outline;

    public void Init(int index, bool enableOutline)
    {
        transform.localPosition = index == 0 ? Vector3.zero : Vector3.up * 1000;
        if (enableOutline)
        {
            outline.enabled = true;
            outline.effectColor = playerSetup.entries[index].color;
        }
    }

    public void UpdateCoins(int coins)
    {
        coinText.text = coins.ToString();
    }
}
