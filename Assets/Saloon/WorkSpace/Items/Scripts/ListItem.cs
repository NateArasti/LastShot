using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite _simpleSprite;
    [SerializeField] private Sprite _chosenSprite;
    [Space(20f)]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _dragItemIcon;

    private Image _background;
    
    public IWorkItem Item { get; private set; }

    private void Start()
    {
        _background = GetComponent<Image>();
        _background.sprite = _simpleSprite;
    }

    public void SetItem(IWorkItem item)
    {
        Item = item;
        _icon.sprite = item.Sprite;
        _dragItemIcon.sprite = item.Sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _background.sprite = _chosenSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _background.sprite = _simpleSprite;
    }
}
