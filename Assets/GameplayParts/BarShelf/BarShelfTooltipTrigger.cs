using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarShelfTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Name { get; set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BarShelfTooltip.TryShow(Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BarShelfTooltip.Hide();
    }

    public void Hide() => OnPointerExit(null);
}