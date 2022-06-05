using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarShelfTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string _name;

    public void OnPointerEnter(PointerEventData eventData)
    {
        BarShelfTooltip.TryShow(_name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BarShelfTooltip.Hide();
    }
}