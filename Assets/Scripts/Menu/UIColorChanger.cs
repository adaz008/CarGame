using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class UIColorChanger : MonoBehaviour
{
    [SerializeField] private Color normalTextColor;
    [SerializeField] private Color hoverTextColor;
    [SerializeField] private Color normalImageColor;
    [SerializeField] private Color hoverImageColor;
    public void OnPointerEnterText(TextMeshProUGUI text)
    {
        text.color = hoverTextColor;
    }

    public void OnPointerExitText(TextMeshProUGUI text)
    {
        text.color = normalTextColor;
    }

    public void OnPointerEnterImage(Image image)
    {
        image.color = hoverImageColor;
    }

    public void OnPointerExitImage(Image image)
    {
        image.color = normalImageColor;
    }

}