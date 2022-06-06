using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class ItemSpace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ItemSpaceNumber
    {
        First,
        Second
    }

    public enum ItemSpaceType
    {
        List,
        Glass,
        SideObject,
        Garnish
    }

    [SerializeField] private ItemSpaceType _type;
    [SerializeField] private ItemSpaceNumber _number;
    [SerializeField] private RectTransform _itemSpawnPivot;
    private bool _canPlace = true;
    private Color _simpleColor;
    private Color _errorColor;
    private Color _highlightedColor;
    private Image _image;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
        (_simpleColor, _highlightedColor, _errorColor) = ItemSpacesStorage.GetColors();
        Disable();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_canPlace) return;
        _image.color = DragItem.CanPlaceCurrent(_type) ? _highlightedColor : _errorColor;
        DragItem.OnDragDrop.AddListener(PlaceItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_canPlace) return;
        _image.color = _simpleColor;
        DragItem.OnDragDrop.RemoveListener(PlaceItem);
    }

    public void DeleteItem()
    {
        _canPlace = true;
    }

    public void ClearSpace()
    {
        foreach (GameObject item in _itemSpawnPivot)
        {
            if(item == gameObject) continue;
            Destroy(item);
        }
    }

    public void PlaceItem(DragItem item)
    {
        if (!_canPlace) return;
        if (item.Instrument != null)
        {
            PlaceInstrument(item.Instrument);
            return;
        }
        if (!item.Item.CanPlaceInThisSpace(_type)) return;
        if (_type == ItemSpaceType.Garnish)
        {
            _canPlace = false;
            _image.sprite = item.Item.GarnishSprite;
            _image.SetNativeSize();
            _image.color = Color.white;
        }

        var spawnWorkItem = item.Item.SpawnWorkItem(_itemSpawnPivot);

        if (item.Item.TakeMousePosition())
        {
            spawnWorkItem.transform.position = item.transform.position;
            if (spawnWorkItem.TryGetComponent<DropItem>(out var dropItem))
            {
                dropItem.TrySpawnDuplicates();
            }
            return;
        }

        spawnWorkItem.transform.SetAsLastSibling();

        spawnWorkItem.GetComponent<PourItem>().SetPosition(_number);
        spawnWorkItem.GetComponent<Returner>().OnReturn.AddListener(DeleteItem);
        _image.color = _simpleColor;
        _canvasGroup.alpha = 0;
        _canPlace = false;
    }

    public void PlaceInstrument(Instrument instrument)
    {
        if (!instrument.CanPlaceInSpace(_type)) return;
        var spawnedInstrument = Instantiate(instrument.Prefab, _itemSpawnPivot);
        if (spawnedInstrument.TryGetComponent<ShakerGlass>(out var shakerGlass))
        {
            spawnedInstrument.transform.SetAsLastSibling();
            _image.color = _simpleColor;
            _canvasGroup.alpha = 0;
            _canPlace = false;
            shakerGlass.SetUp(DeleteItem, _number);
        }
    }

    public void Enable()
    {
        if (!_canPlace) return;
        _canvasGroup.alpha = 1;
    }

    public void Disable()
    {
        if(!_canPlace) return;
        _canvasGroup.alpha = 0;
    }
}