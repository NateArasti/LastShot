using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    private const float ForceModificator = 0.1f;

    [SerializeField] private float _randomSpawnRadius = 2;

    private CircleCollider2D _collider;
    private Image _image;
    private float _volume;
    private RectTransform _rectTransform;
    private Rigidbody2D _rigidbody2D;
    private int _duplicatesCount;
    private bool _useRBForce = true;

    public float Volume
    {
        get
        {
            var res = _volume;
            _volume = 0;
            return res;
        }
    }

    public float Force => _useRBForce ? _rigidbody2D.velocity.magnitude * ForceModificator : 0;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _rigidbody2D.gravityScale *= 0.75f;
    }

    public void SetItem(Sprite icon, float mass, float volume, int duplicatesCount, float delayBeforeRBTurnOff)
    {
        _volume = volume;
        _image.sprite = icon;
        _image.SetNativeSize();
        _rigidbody2D.mass = mass;
        _duplicatesCount = duplicatesCount;
        _collider.radius = 0.5f * Mathf.Min(_rectTransform.rect.width, _rectTransform.rect.height);
        transform.SetAsFirstSibling();
        if(delayBeforeRBTurnOff == 0) return;
        StartCoroutine(RBTurnOff(delayBeforeRBTurnOff));
    }

    public void TrySpawnDuplicates()
    {
        for (var i = 0; i < _duplicatesCount; i++)
        {
            var randomPosition = (Vector2)transform.position + Random.insideUnitCircle * _randomSpawnRadius;
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360));
            Instantiate(this, randomPosition, randomRotation, transform.parent)._useRBForce = false;
        }
    }

    private IEnumerator RBTurnOff(float delay)
    {
        yield return UnityExtensions.Wait(delay);
        Destroy(_rigidbody2D);
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