using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ItemSpace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ItemSpaceType
    {
        List,
        Glass,
        SideObject,
        Garnish
    }

    [SerializeField] private ItemSpaceType _type;
    [SerializeField] private RectTransform _itemSpawnPivot;

    private Color _simpleColor;
    private Color _highlightedColor;
    private Color _errorColor;

    private Image _image;

    private bool _canPlace = true;
    private Liquid _connectedLiquid;

    public void PlaceItem(DragItem item)
    {
        if (_type == ItemSpaceType.List) return;
        var spawnedItem = item.Item.SpawnWorkItem(_itemSpawnPivot);
        spawnedItem.transform.SetAsFirstSibling();
        if (item.Item.TakeMousePosition())
            spawnedItem.transform.position = item.transform.position;
        if(_type == ItemSpaceType.SideObject)
        {
            _canPlace = false;
            spawnedItem.GetComponent<Returner>().OnReturn.AddListener(() => DeleteItem());
            if (spawnedItem.GetComponent<MixGlass>() != null)
            {
                _type = ItemSpaceType.Glass;
                _canPlace = true;
            }
        }
        if(spawnedItem.TryGetComponent<CocktailSpoon>(out var cocktailSpoon))
        {
            cocktailSpoon.ConnectToLiquid(_connectedLiquid);
            var data = transform.parent.GetComponentInChildren<HandInstrumentData>();
            if (data != null)
                cocktailSpoon.SetMoveData(data.SpoonData.offSet, data.SpoonData.xClamp);
        }
        if (spawnedItem.TryGetComponent<DropItem>(out var dropItem))
        {
            dropItem.ConnectToLiquid(_connectedLiquid);
        }
    }

    public void ConnectLiquid(Liquid liquid)
    {
        _connectedLiquid = liquid;
    }

    public void DeleteItem()
    {
        _canPlace = true;
        _connectedLiquid = null;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        (_simpleColor, _highlightedColor, _errorColor) = ItemSpacesStorage.GetColors();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _simpleColor;
        if (DragItem.CurrentInstance != null)
        {
            DragItem.CurrentInstance.SetSpace(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DragItem.CurrentInstance != null)
        {
            if (DragItem.CurrentInstance.Item.CanPlaceInThisSpace(_type) && _canPlace)
            {
                _image.color = _highlightedColor;
                DragItem.CurrentInstance.SetSpace(this);
            }
            else
                _image.color = _errorColor;
        }
    }
}
