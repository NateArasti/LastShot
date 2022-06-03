using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GarnishSpace : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //private ItemSpace.ItemSpaceType _type = ItemSpace.ItemSpaceType.Garnish;
    public bool Garnished { get; private set; }

    private Color _simpleColor;
    private Color _highlightedColor;
    private Color _errorColor;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        (_simpleColor, _highlightedColor, _errorColor) = ItemSpacesStorage.GetColors();
    }

    public void Garnish(Sprite sprite)
    {
        Garnished = true;
        ItemSpacesStorage.DisconnectGarnishSpaces();
        _image.type = Image.Type.Simple;
        _image.sprite = sprite;
        _image.SetNativeSize();
        _image.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (Garnished) return;
        //_image.color = _simpleColor;
        //if (DragItem.CurrentInstance != null)
        //{
        //    DragItem.CurrentInstance.SetGarnishSpace(null);
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (DragItem.CurrentInstance != null && !Garnished)
        //{
        //    if (DragItem.CurrentInstance.Item.CanPlaceInThisSpace(_type))
        //    {
        //        _image.color = _highlightedColor;
        //        DragItem.CurrentInstance.SetGarnishSpace(this);
        //    }
        //    else
        //        _image.color = _errorColor;
        //}
    }
}
