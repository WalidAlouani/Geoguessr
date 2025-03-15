using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerTopBar : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    public void UpdateCoins(int coins)
    {
        coinText.text = coins.ToString();
    }
}
