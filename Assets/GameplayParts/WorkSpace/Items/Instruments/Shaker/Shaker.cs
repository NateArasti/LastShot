using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Returner))]
public class Shaker : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [Header("Pour Params")]
    [SerializeField] private PourItem _pourShaker;
    [SerializeField] private float _delayBetweenDrops;
    [SerializeField] private Sprite _pourSprite;
    [Header("Shake Params")]
    [SerializeField] private RectTransform _dragTransform;
    [SerializeField] private Slider _intensitySlider;
    [SerializeField] private float _deltaApplier = 1f;
    [SerializeField] private float _resistance = 1f;
    [SerializeField] private float _smoothingApplier = 2f;
    [SerializeField] private Gradient _gradient;
    private UnityAction _endAction;
    private Canvas _canvas;
    private Returner _returner;
    private Vector3 _startPosition;

    private ItemSpace.ItemSpaceNumber _number;

    private Color _startColor;

    private void Awake()
    {
        _returner = GetComponent<Returner>();
        _canvas = ItemSpacesStorage.Canvas;
    }

    private void Update()
    {
        _intensitySlider.value -= _resistance * Time.deltaTime;
        var (colorKeys, alphaKeys) = 
            LiquidRenderer.GetGradientSmoothing(_intensitySlider.value * 
                                                _smoothingApplier * Time.deltaTime, 
                                                _gradient);
        _gradient.SetKeys(colorKeys, alphaKeys);
    }

    public void SetUp(Gradient gradient, UnityAction endAction, ItemSpace.ItemSpaceNumber number)
    {
        _gradient = gradient;
        _startColor = _gradient.Evaluate(1);
        _number = number;
        _endAction = endAction;
        _intensitySlider.value = 0;
        GetComponent<Returner>().OnReturn.AddListener(OnEnd);
        _startPosition = _dragTransform.position;
    }

    private void OnEnd()
    {
        var endColor = _gradient.Evaluate(1);
        var (colorKeys, alphaKeys) =
            LiquidRenderer.GetGradientSmoothing(100,
                                                _gradient);
        var distance = _startColor.GetDistanceTo(colorKeys[^1].color);
        var passed = _startColor.GetDistanceTo(endColor);
        OrderCreationEvents.Instance.OrderActionsTracker.AddAction(new OrderAction.ShakerMixAction(false)
        {
            Intensity = passed / distance
        });

        var pour = Instantiate(_pourShaker, transform.parent);
        pour.SetItem(_pourSprite, endColor, _delayBetweenDrops, 60, "SHAKER");
        pour.SetPosition(_number);
        pour.GetComponent<Returner>().OnReturn.AddListener(_endAction);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var delta = eventData.delta.y / _canvas.scaleFactor;
        _dragTransform.anchoredPosition += Vector2.up * delta;
        _intensitySlider.value += Mathf.Abs(delta) * _deltaApplier * Time.deltaTime;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _returner.enabled = true;
        _dragTransform.position = _startPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _returner.enabled = false;
    }
}
