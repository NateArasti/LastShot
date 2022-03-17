using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CocktailSpoon : MonoBehaviour, IWorkItem, IDragHandler
{
    public readonly UnityEvent<float> OnStir = new UnityEvent<float>();

    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _borderAngle;

    private Vector2 _offSet = Vector2.zero;
    private float _xClamp;

    private float _rotationStep;
    private RectTransform _rectTransform;

    private float _previousX;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ConnectToLiquid(Liquid liquid)
    {
        OnStir.AddListener((intesity) => Liquid.StirAllConnectedLiquids(intesity, liquid));
    }

    public void SetMoveData(Vector2 offSet, float xClamp)
    {
        _offSet = offSet;
        _xClamp = xClamp;
        _rectTransform.anchoredPosition = _offSet;
        _rotationStep = _borderAngle / _xClamp;
    }

    private void Update()
    {
        var newX = _rectTransform.anchoredPosition.x;
        var intensity = Mathf.Abs(newX - _previousX) / _xClamp;
        if(intensity > 0)
        {
            OnStir.Invoke(intensity);
        }
        _previousX = newX;
    }

    public Sprite Sprite => _sprite;

    public bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) => 
        type == ItemSpace.ItemSpaceType.List ||
        type == ItemSpace.ItemSpaceType.Glass;

    public void OnDrag(PointerEventData eventData)
    {
        var newX = _rectTransform.anchoredPosition.x - _offSet.x + eventData.delta.x / ItemSpacesStorage.Canvas.scaleFactor;
        _rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(newX, - _xClamp, _xClamp), 0) + _offSet;
        _rectTransform.localEulerAngles = new Vector3(0, (_rectTransform.anchoredPosition.x - _offSet.x) * _rotationStep, 0);
    }

    public GameObject SpawnWorkItem(Transform container) => Instantiate(gameObject, container);

    public bool TakeMousePosition() => false;
}
