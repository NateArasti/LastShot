using System.Security.Cryptography.X509Certificates;
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

    public enum ItemSpaceNumber
    {
        First,
        Second
    }

    [SerializeField] private ItemSpaceType _type;
    [SerializeField] private ItemSpaceNumber _number;
    [SerializeField] private RectTransform _itemSpawnPivot;

    private Color _simpleColor;
    private Color _highlightedColor;
    private Color _errorColor;

    private Image _image;

    [SerializeField]private bool _canPlace = true;

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

    private void Awake()
    {
        _image = GetComponent<Image>();
        (_simpleColor, _highlightedColor, _errorColor) = ItemSpacesStorage.GetColors();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = _simpleColor;
        DragItem.OnDragDrop.RemoveListener(PlaceItem);
        //_canPlace = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DragItem.OnDragDrop.AddListener(PlaceItem);

        //_canPlace = true;
        _image.color = _highlightedColor;
    }
}
