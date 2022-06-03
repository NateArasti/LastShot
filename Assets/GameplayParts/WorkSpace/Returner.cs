using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Returner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public readonly UnityEvent OnReturn = new();

    private bool _mouseOnObject;

    private void Update()
    {
        if(_mouseOnObject && Input.GetMouseButtonDown(1))
        {
            OnReturn.Invoke();
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOnObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOnObject = false;
    }
}
