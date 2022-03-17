using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image), typeof(Returner))]
public class PourItem : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private static int LastIndex;
    private int _index;

    [SerializeField] private Vector2 _minMaxAngles = new Vector2(0, 45);
    [SerializeField] private float _rotationSpeed = 1;
    [SerializeField] private bool _right;
    [SerializeField] private RectTransform _spawnPivot;

    private LiquidDropSpawner _dropSpawner;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private Image _image;
    private Returner _returner;
    private Vector2 _startPosition;
    private Vector2 _startPivot;

    private bool _isDragging;
    private bool _endedDragging;
    private bool _isRotating;
    private float _zAngle;

    private (Color mainColor, Color outlineColor) _colors;
    private float _delayBetweenDrops;

    public void SetItem(Sprite icon, (Color mainColor, Color outlineColor) colors, float delayBetweenSpawn)
    {
        _image.sprite = icon;
        _image.SetNativeSize();
        _colors = colors;
        _delayBetweenDrops = delayBetweenSpawn;
    }

    private void Awake()
    {
        LastIndex += 1;
        _index = LastIndex;

        _rectTransform = GetComponent<RectTransform>();
        _startPosition = _rectTransform.anchoredPosition;
        _startPivot = _rectTransform.pivot;
        _image = GetComponent<Image>();
        _canvas = ItemSpacesStorage.Canvas;
        _dropSpawner = LiquidDropSpawner.Instance;
        _returner = GetComponent<Returner>();
    }

    private void Update()
    {
        if(_isDragging || _isRotating)
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(StartRotating());
                _isRotating = true;
            }
            if(Input.GetMouseButtonUp(1))
            {
                EndRotation();
            }
        }
    }

    private void EndRotation()
    {
        StopAllCoroutines();
        _isRotating = false;
        _zAngle = 0;
        _rectTransform.eulerAngles = Vector3.zero;
        StopDrops();
    }

    private IEnumerator StartRotating()
    {
        while (true)
        {
            _zAngle = Mathf.Clamp(_zAngle + _rotationSpeed * Time.deltaTime, _minMaxAngles.x, _minMaxAngles.y);
            if (_right)
            {
                _rectTransform.rotation = Quaternion.Euler(0, 0, -_zAngle);
            }
            else
            {
                _rectTransform.rotation = Quaternion.Euler(0, 0, _zAngle);
            }
            if ((_right && Mathf.Approximately(360 - _rectTransform.eulerAngles.z, _minMaxAngles.y)) ||
                (!_right && Mathf.Approximately(_rectTransform.eulerAngles.z, _minMaxAngles.y)))
            {
                SpawnDrops();
            }
            yield return null;
        }
    }

    private void SpawnDrops()
    {
        if (_dropSpawner.Spawning) return;
        _dropSpawner.transform.position = _spawnPivot.position;
        _dropSpawner.SetParams(_colors, _delayBetweenDrops, _index, _right);
        _dropSpawner.Spawning = true;
    }

    private void StopDrops()
    {
        _dropSpawner.Spawning = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _returner.enabled = false;
        var previosPivot = _rectTransform.pivot;
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.anchoredPosition += new Vector2(
            0.5f * Mathf.Abs(_rectTransform.pivot.x - previosPivot.x) * _rectTransform.rect.width,
            Mathf.Abs(_rectTransform.pivot.y - previosPivot.y) * _rectTransform.rect.height);
        _endedDragging = false;
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isRotating || _endedDragging) return;
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _returner.enabled = true;
        _rectTransform.pivot = _startPivot;
        _rectTransform.anchoredPosition = _startPosition;
        EndRotation();
        _endedDragging = true;
        _isDragging = false;
    }
}
