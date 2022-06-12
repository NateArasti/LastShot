using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PourItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static string PouringItemKeyName { get; private set; }

    [SerializeField] private Vector2 _minMaxAngles = new(0, 45);
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private float _dropSpawnStartAngle = 70f;
    [SerializeField] private float _delayBeforeRepeatRotation = .1f;
    [SerializeField] private RectTransform _spawnPivot;
   
    private bool _isDragging;
    private bool _isRotating;
    public bool CanSpawnDrops { get; set; } = false;
    private bool _needPauseInRotation;

    private Color _color;
    private float _minDelayBetweenDrops;
    private float _flowForce; // сила отклонения струи
    private float _rotationAngle;
    private bool _right;

    private Image _image;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Returner _returner;
    private Coroutine _rotationCoroutine;
    private DropSpawner _spawner;
    private Vector2 _startPosition;
    private Vector2 _jetDirection;

    private float _currentVolume;
    private string _itemKeyName;

    private float ZAngle
    {
        set
        {
            _rotationAngle = value;
            _rectTransform.rotation = Quaternion.Euler(0, 0, _rotationAngle);
        }

        get => _rotationAngle;
    }


    private void Awake()
    {
        _spawner = GameObject.FindGameObjectWithTag("DropSpawner").GetComponent<DropSpawner>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = ItemSpacesStorage.Canvas;
        _returner = GetComponent<Returner>();
        _image = GetComponent<Image>();
        _flowForce = 10f;
    }

    private void Update()
    {
        if (_isDragging)
        {
            _spawner.DropData = (_spawnPivot.position, _jetDirection);
            _spawner.DropColor = _color;
            if (Input.GetMouseButtonUp(0))
                OnEndDrag(null);

            if (Input.GetMouseButtonDown(1))
            {
                _needPauseInRotation = Math.Abs(ZAngle) > 0;
                ZAngle = 0;
                _spawner.IsDropping = false;
                _isRotating = true;
            }

            if (_isRotating && Input.GetMouseButtonUp(1)) _isRotating = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_rotationCoroutine != null)
            StopCoroutine(_rotationCoroutine);
        _rotationCoroutine = StartCoroutine(Rotation());
        _returner.enabled = false;
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData != null)
            return;

        _spawner.IsDropping = false;
        _returner.enabled = true;
        _isDragging = false;
        ZAngle = 0;
        _isRotating = false;
        if (_rotationCoroutine != null)
            StopCoroutine(_rotationCoroutine);
        _rectTransform.anchoredPosition = _startPosition;
        _rectTransform.eulerAngles = Vector3.zero;
    }

    public void SetItem(Sprite icon, Color color, float minDelayBetweenDrops, float totalVolume, string keyName)
    {
        _currentVolume = totalVolume;
        _itemKeyName = keyName;

        _color = color;
        _minDelayBetweenDrops = minDelayBetweenDrops;

        _image.sprite = icon;
        _image.SetNativeSize();
        _rectTransform.anchoredPosition += _rectTransform.rect.height * 0.5f * Vector2.up;
        _rectTransform.pivot = Vector2.one * 0.5f;
        _startPosition = _rectTransform.anchoredPosition;
    }

    public void SetPosition(ItemSpace.ItemSpaceNumber number)
    {
        _right = number != ItemSpace.ItemSpaceNumber.First;
    }

    private IEnumerator Rotation()
    {
        while (true)
        {
            yield return null;
            _currentVolume -= _spawner.GetSpawnedDelta() * WaterDrop.Mass;
            if(_currentVolume <= 0)
            {
                _spawner.IsDropping = false;
                _spawner.DropDelay = 0;
                if (PouringItemKeyName == "SHAKER")
                    OrderCreationEvents.Instance.OrderActionsTracker.AddAction(new OrderAction.ShakerPourAction());
                yield break;
            }
            if (CanSpawnDrops && Mathf.Abs(ZAngle) > 1)
            {
                _spawner.IsDropping = Mathf.Abs(ZAngle) > _dropSpawnStartAngle;
                _spawner.DropDelay = _minDelayBetweenDrops + 
                                     Mathf.Clamp01(4 * _minDelayBetweenDrops) * 
                                     (1 - Mathf.Abs(ZAngle) / _minMaxAngles.y);
            }
            else
            {
                _spawner.IsDropping = false;
                _spawner.DropDelay = 0;
            }

            PouringItemKeyName = _spawner.IsDropping ? _itemKeyName : string.Empty;

            if (_needPauseInRotation)
                yield return UnityExtensions.Wait(_delayBeforeRepeatRotation);
            _needPauseInRotation = false;


            if (!_isRotating)
                continue;

            if (_right)
            {
                ZAngle = Mathf.Clamp(ZAngle + _rotationSpeed * Time.deltaTime, _minMaxAngles.x, _minMaxAngles.y);
                _jetDirection = ZAngle < _minMaxAngles.y / 2
                    ? new Vector2(Mathf.Lerp(0, -_flowForce, ZAngle * 2 / _minMaxAngles.y), 0)
                    : new Vector2(Mathf.Lerp(-_flowForce, 0, ZAngle / _minMaxAngles.y), 0);
            }
            else
            {
                ZAngle = Mathf.Clamp(ZAngle - _rotationSpeed * Time.deltaTime, -_minMaxAngles.y, _minMaxAngles.x);
                _jetDirection = -ZAngle < _minMaxAngles.y / 2
                    ? new Vector2(Mathf.Lerp(0, _flowForce, -ZAngle * 2 / _minMaxAngles.y), 0)
                    : new Vector2(Mathf.Lerp(_flowForce, 0, -ZAngle / _minMaxAngles.y), 0);
            }
        }
    }
}