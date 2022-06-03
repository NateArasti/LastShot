using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    private BoxCollider2D _collider;
    private Image _image;
    private float _mass;
    private RectTransform _rectTransform;
    private Rigidbody2D _rigidbody2D;

    public float Mass
    {
        get
        {
            var res = _mass;
            return res;
        }
    }


    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _rigidbody2D.gravityScale = 0.8f;
    }

    public void SetItem(Sprite icon, float mass, bool floating)
    {
        //_float = floating;
        _mass = mass;
        _image.sprite = icon;
        //_mass = _rectTransform.rect.width * _rectTransform.rect.height / 2000;
        _collider.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
        transform.SetAsFirstSibling();
    }

    #region Floating

    //public void TryStartFloating()
    //{
    //    if (!_float) return;
    //    StartCoroutine(Float());
    //}


    //private IEnumerator Float()
    //{
    //var forceMultiplier = -1f;
    //while (forceMultiplier > -5 || _a)
    //{
    //    _a = false;
    //     forceMultiplier = LiquidTrigger.GetForceMultiplier(transform.position.y);
    //    //MathF.Sqrt(MathF.Abs(transform.position.y - _triggerBounds))*5;//Liquid.GetFloatMultiplier(transform.position.y, _connectedLiquid);
    //    print(forceMultiplier);
    //    if (forceMultiplier > 0)
    //        _rigidbody2D.AddForce(-Physics.gravity * (forceMultiplier - _rigidbody2D.velocity.y * 0.01f));
    //yield return new WaitForFixedUpdate();
    //}
    //}

    #endregion
}