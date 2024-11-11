using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform buttonRectTransform;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f); // Scale when hovered
    private Vector3 originalScale;

    void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
        originalScale = buttonRectTransform.localScale; // Store the original scale
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonRectTransform.localScale = hoverScale; // Scale up on hover
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonRectTransform.localScale = originalScale; // Reset to original scale
    }
}
