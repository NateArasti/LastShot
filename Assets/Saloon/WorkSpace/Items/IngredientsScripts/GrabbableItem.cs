using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GrabbableItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private GrabDrop _grabDrop;
    [SerializeField] private float _gravityScale = 5;
    private RectTransform _rectTransform;
    private Canvas _canvas;

    private float _mass;
    private Color _color;

    private void Awake()
    {
        _canvas = ItemSpacesStorage.Canvas;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetItem(float mass, Color color)
    {
        _mass = mass;
        _color = color;
        _grabDrop.SetItem(color, mass);
        _grabDrop.Hide();
        _grabDrop.OnDestroy += () => Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Instantiate(gameObject, transform.parent).GetComponent<GrabbableItem>().SetItem(_mass, _color);
        _grabDrop.Show();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _grabDrop.gameObject.AddComponent<Rigidbody2D>().gravityScale = _gravityScale;
    }
}
