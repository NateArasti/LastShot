using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup), typeof(Image))]
public class DragItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static DragItem CurrentInstance { get; private set; }

    private ItemSpace _chosenWorkSpace;
    private GarnishSpace _garnishSpace;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private ListItem _parentListItem;

    public IWorkItem Item => _parentListItem.Item;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = ItemSpacesStorage.Canvas;
        _parentListItem = transform.parent.GetComponent<ListItem>();
    }

    public void SetSpace(ItemSpace space)
    {
        if(CurrentInstance != null)
        {
            _chosenWorkSpace = space;
            _garnishSpace = null;
        }
    }

    public void SetGarnishSpace(GarnishSpace space)
    {
        if (CurrentInstance != null)
        {
            _garnishSpace = space;
            _chosenWorkSpace = null;
        }
    }

    private void StayInChosenSpace()
    {
        if (_chosenWorkSpace != null)
            _chosenWorkSpace.PlaceItem(this);
        else if (_garnishSpace != null)
            _garnishSpace.Garnish(Item.Sprite);
        Destroy(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CurrentInstance = null;
        _canvasGroup.blocksRaycasts = true;
        StayInChosenSpace();
        ItemSpacesStorage.SetSpacesActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Instantiate(gameObject, transform.parent);
        GetComponent<Image>().SetNativeSize();
        _parentListItem.OnPointerExit(null);
        transform.SetParent(_canvas.transform);
        CurrentInstance = this;
        _canvasGroup.blocksRaycasts = false;
        ItemSpacesStorage.SetSpacesActive(true);
    }
}
