using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PourItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private Vector2 _minMaxAngles = new Vector2(0, 45);
    [SerializeField] private float _rotationSpeed = 1f;
    [SerializeField] private bool _right;
    [SerializeField] private RectTransform _spawnPivot;


    private RectTransform _rectTransform;
    private Returner _returner; 
    private Canvas _canvas;
    private Vector2 _startPivot;
    private Vector2 _startPosition;
    private Vector2 _jetDirection;
    private Image _image;
    private Coroutine _rotationCoroutine;
   
    private DropSpawner _spawner;

    private float _rotationAngle;

    private float _flowForce; // сила отклонения струи
    private float _timeDelay; // время между дропами

    private bool _canSpawnDrops;
    private bool _isDragging;
    private bool _isRotating;

    private float ZAngle
    {
        set
        {
            _rotationAngle = value;
            _rectTransform.rotation = Quaternion.Euler(0, 0, _rotationAngle);
        }

        get => _rotationAngle;
    }

    private (Color mainColor, Color outlineColor) _colors;

    private bool _needPauseInRotation;


    private void Awake()
    {
        _spawner = GameObject.FindGameObjectWithTag("DropSpawner").GetComponent<DropSpawner>();
        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
        _startPivot = _rectTransform.pivot;
        _canvas = ItemSpacesStorage.Canvas;
        _returner = GetComponent<Returner>();
        _image = GetComponent<Image>();
        _canSpawnDrops = true;
        _flowForce = 10f;
    }

    public void SetItem(Sprite icon)//, (Color mainColor, Color outlineColor) colors)
    {
        _image.sprite = icon;
        _image.SetNativeSize();
        //_colors = colors;
    }

    public void SetPosition(ItemSpace.ItemSpaceNumber number)
    {
        _right = number != ItemSpace.ItemSpaceNumber.First;
    }

    private void Update()
    {

        if (_isDragging)
        {
            //print(_jetDirection.ToString());
            _spawner.DropData = (_spawnPivot.position, _jetDirection);

            if (Input.GetMouseButtonUp(0))
                OnEndDrag(null);

            if (Input.GetMouseButtonDown(1))
            {
                _needPauseInRotation = Math.Abs(ZAngle) > 0;
                ZAngle = 0;
                _spawner.IsDropping = false;
                _isRotating = true;
            }

            if (_isRotating && Input.GetMouseButtonUp(1))
            {
                _isRotating = false;
            }
            
        }
    }

    private IEnumerator Rotation()
    {
        while (true)
        {
            yield return null;

            if (_canSpawnDrops && Math.Abs(ZAngle) >1)
            {
                _spawner.IsDropping = Math.Abs(ZAngle) > 40;
                _spawner.DropDelay = Math.Abs(ZAngle / (ZAngle * ZAngle));
            }

            if (_needPauseInRotation)
                yield return UnityExtensions.Wait(1);


            if (!_isRotating)
                continue;

            if (_right)
            {
                ZAngle = Mathf.Clamp(ZAngle + _rotationSpeed * Time.deltaTime, _minMaxAngles.x, _minMaxAngles.y);
                _jetDirection = ZAngle < _minMaxAngles.y / 2 ? new Vector2(Mathf.Lerp(0, -_flowForce, ZAngle * 2 /_minMaxAngles.y  ), 0) : new Vector2(Mathf.Lerp(-_flowForce, 0, ZAngle / _minMaxAngles.y), 0);
            }
            else
            {
                ZAngle = Mathf.Clamp(ZAngle - _rotationSpeed * Time.deltaTime, -_minMaxAngles.y, _minMaxAngles.x);
                _jetDirection = -ZAngle < _minMaxAngles.y / 2 ? new Vector2(Mathf.Lerp(0, _flowForce, -ZAngle*2 / _minMaxAngles.y), 0) : new Vector2(Mathf.Lerp(_flowForce, 0, -ZAngle / _minMaxAngles.y), 0);
            }
        }
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
        _rectTransform.pivot = _startPivot;
        _rectTransform.anchoredPosition = _startPosition;
        _rectTransform.eulerAngles = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_rotationCoroutine != null)
            StopCoroutine(_rotationCoroutine);
        _rotationCoroutine = StartCoroutine(Rotation());

        _returner.enabled = false;
        _isDragging = true;

        //var previosPivot = _rectTransform.pivot;
        //_rectTransform.pivot = new Vector2(0.5f, 0.5f);
        //_rectTransform.anchoredPosition += new Vector2(
        //    0.5f * Mathf.Abs(_rectTransform.pivot.x - previosPivot.x) * _rectTransform.rect.width,
        //    Mathf.Abs(_rectTransform.pivot.y - previosPivot.y) * _rectTransform.rect.height);
        //_endedDragging = false;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _canSpawnDrops = false;
        _spawner.IsDropping = false;

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _canSpawnDrops = true;
        //_spawner.IsDropping = true;
    }
}
