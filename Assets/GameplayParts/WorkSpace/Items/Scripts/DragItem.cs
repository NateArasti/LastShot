using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(
    typeof(RectTransform),
    typeof(CanvasGroup), 
    typeof(Image))]
public class DragItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private static DragItem _currentDragItem;
    public static UnityEvent<DragItem> OnDragDrop = new();

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private ListItem _parentListItem;
    private RectTransform _rectTransform;
    private Image _image;

    public Instrument Instrument { get; private set; }

    public Ingredient Item => _parentListItem.Item;

    public static bool CanPlaceCurrent(ItemSpace.ItemSpaceType type) => 
        _currentDragItem != null &&
        (_currentDragItem.Item != null && _currentDragItem.Item.CanPlaceInThisSpace(type)
         ||
        _currentDragItem.Instrument != null && _currentDragItem.Instrument.CanPlaceInSpace(type));

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _canvas = ItemSpacesStorage.Canvas;
        _canvasGroup = GetComponent<CanvasGroup>();
        _parentListItem = transform.parent.GetComponent<ListItem>();
        _canvasGroup.alpha = 0;
    }

    private void Start()
    {
        var check = _parentListItem as InstrumentListItem;
        Instrument = check == null ? null : check.Instrument;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Awake();
        _currentDragItem = this;
        Instantiate(gameObject, transform.parent);
        _image.SetNativeSize();
        _rectTransform.SetParent(_canvas.transform);
        _rectTransform.SetAsLastSibling();
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 1;
        ItemSpacesStorage.SetSpacesActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //_currentDragItem = null;
        OnDragDrop.Invoke(this);
        OnDragDrop.RemoveAllListeners();
        ItemSpacesStorage.SetSpacesActive(false);
        Destroy(gameObject);
    }
}