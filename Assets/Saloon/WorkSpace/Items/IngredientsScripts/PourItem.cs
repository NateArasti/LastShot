using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private Image _image;
   
    private DropSpawner _spawner;

    private float _rotationAngle;

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
        StartCoroutine(Rotation());
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
            _spawner.DropData = (_spawnPivot.position, Vector2.down);

            if (Input.GetMouseButtonUp(0))
                OnEndDrag(null);

            if (Input.GetMouseButtonDown(1))
            {
                //print(_spawner.DropData.position);
                _needPauseInRotation = Math.Abs(ZAngle) > 0;
                ZAngle = 0;
                _spawner.IsDropping = false;
                print("STOP!!!");
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

            if (_needPauseInRotation)
                yield return UnityExtensions.Wait(1);

            _spawner.IsDropping = Math.Abs(ZAngle) > 40;
            print("Droping!!!!!!");

            if (!_isRotating)
                continue;

            ZAngle = _right
                ? Mathf.Clamp(ZAngle + _rotationSpeed * Time.deltaTime, _minMaxAngles.x, _minMaxAngles.y)
                : Mathf.Clamp(ZAngle - _rotationSpeed * Time.deltaTime, -_minMaxAngles.y, _minMaxAngles.x);
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

        _rectTransform.pivot = _startPivot;
        _rectTransform.anchoredPosition = _startPosition;
        _rectTransform.eulerAngles = Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        _returner.enabled = false;
        _isDragging = true;

        //var previosPivot = _rectTransform.pivot;
        //_rectTransform.pivot = new Vector2(0.5f, 0.5f);
        //_rectTransform.anchoredPosition += new Vector2(
        //    0.5f * Mathf.Abs(_rectTransform.pivot.x - previosPivot.x) * _rectTransform.rect.width,
        //    Mathf.Abs(_rectTransform.pivot.y - previosPivot.y) * _rectTransform.rect.height);
        //_endedDragging = false;

    }
}
