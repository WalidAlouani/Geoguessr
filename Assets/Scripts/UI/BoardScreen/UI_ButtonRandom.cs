using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonRandom : MonoBehaviour
{
    enum ButtonRandomState { Idle, Randamizing, Found }

    [SerializeField] private Button button;
    [SerializeField] private Image image;

    private void OnPointerDown() 
    {
        image.enabled = false;
    }

    private void OnPointerUp()
    {
        image.enabled = true;
    }
}
