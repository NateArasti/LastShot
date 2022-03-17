using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class DropItem : MonoBehaviour
{
    private float _mass;
    private bool _float;
    private Rigidbody2D _rigidbody2D;
    private Liquid _connectedLiquid;

    private int _triggerCount;

    public float Velocity => 0.01f * _rigidbody2D.velocity.magnitude;
    public float Mass
    {
        get
        {
            var res = _mass;
            _mass = 0;
            return res;
        }
    }

    private Image _image;
    private RectTransform _rectTransform;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ConnectToLiquid(Liquid liquid)
    {
        _connectedLiquid = liquid;
        _connectedLiquid.OnDestroyEvent.AddListener(() =>
        {
            _connectedLiquid = null;
            _triggerCount = 0;
        });
    }

    public void SetItem(Sprite icon, float mass, bool floating)
    {
        _float = floating;
        _mass = mass;
        _image.sprite = icon;
        _image.SetNativeSize();
        _mass = _rectTransform.rect.width * _rectTransform.rect.height / 1000;
        _collider.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
        transform.SetAsFirstSibling();
    }

    public void TryStartFloating()
    {
        if (!_float) return;
        StartCoroutine(Float());
    }

    private IEnumerator Float()
    {
        while (_triggerCount > 0)
        {
            var forceMultiplier = Liquid.GetFloatMultiplier(transform.position.y, _connectedLiquid);
            if (forceMultiplier > 0)
                _rigidbody2D.AddForce(-Physics.gravity * (forceMultiplier - _rigidbody2D.velocity.y * 0.01f));
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _triggerCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _triggerCount--;
    }
}
