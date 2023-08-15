using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string defaultText = "Default";
    public string hoverText = "Hovered!";
    private TMP_Text buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
        buttonText.text = defaultText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.text = hoverText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.text = defaultText;
    }
}