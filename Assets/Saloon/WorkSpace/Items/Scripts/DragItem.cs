using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup), typeof(Image))]
public class DragItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static UnityEvent<DragItem> OnDragDrop = new();
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private ListItem _parentListItem;
    private RectTransform _rectTransform;
    public Ingredient Item => _parentListItem.Item;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GameObject.FindGameObjectWithTag("WorkSpaceCanvas").GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _parentListItem = transform.parent.GetComponent<ListItem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Instantiate(gameObject, transform.parent);
        GetComponent<Image>().SetNativeSize();
        transform.SetParent(_canvas.transform);
        _canvasGroup.blocksRaycasts = false;
        ItemSpacesStorage.SetSpacesActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragDrop.Invoke(this);
        OnDragDrop.RemoveAllListeners();
        ItemSpacesStorage.SetSpacesActive(false);
        Destroy(gameObject);
    }
}