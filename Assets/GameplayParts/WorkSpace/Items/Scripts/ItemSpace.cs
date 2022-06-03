using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
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

    [SerializeField] private bool _canPlace = true;
    private Color _errorColor;
    private Color _highlightedColor;

    private Image _image;

    private Color _simpleColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        (_simpleColor, _highlightedColor, _errorColor) = ItemSpacesStorage.GetColors();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragItem.OnDragDrop.AddListener(PlaceItem);
        _image.color = _highlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _simpleColor;
        DragItem.OnDragDrop.RemoveListener(PlaceItem);
    }

    public void DeleteItem()
    {
        _canPlace = true;
    }

    public void PlaceItem(DragItem item)
    {
        if (_canPlace && item.Item.CanPlaceInThisSpace(_type))
        {
            var spawnWorkItem = item.Item.SpawnWorkItem(_itemSpawnPivot);

            if (item.Item.TakeMousePosition())
            {
                spawnWorkItem.transform.position = item.transform.position;
                return;
            }

            spawnWorkItem.GetComponent<PourItem>().SetPosition(_number);
            spawnWorkItem.GetComponent<Returner>().OnReturn.AddListener(DeleteItem);
            _canPlace = false;
        }
    }
}