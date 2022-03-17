using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Returner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public readonly UnityEvent OnReturn = new UnityEvent();

    private bool mouseOnObject;

    private void Update()
    {
        if(mouseOnObject && Input.GetMouseButtonDown(1))
        {
            OnReturn.Invoke();
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOnObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnObject = false;
    }
}
