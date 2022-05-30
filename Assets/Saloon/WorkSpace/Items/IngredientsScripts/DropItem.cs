using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{

    private float _mass;
    private bool _float;
    private Rigidbody2D _rigidbody2D;
    private int _triggerCount;
    private float _triggerBounds;
    private Image _image;
    private RectTransform _rectTransform;
    private BoxCollider2D _collider;
    private StaticLiquid _liquid;
    private BoxCollider2D _waterBoxCollider2D;
    

    //private bool _a = true;

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




    private void Awake()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //if (_triggerCount > 0 && _a)
        //{
        //    TryStartFloating();
        //}
    }

    public void SetItem(Sprite icon, float mass, bool floating)
    {
        _float = floating;
        _mass = mass;
        _image.sprite = icon;
        _image.SetNativeSize();
        _mass = _rectTransform.rect.width * _rectTransform.rect.height / 2000;
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
       
        //var forceMultiplier = -1f;
        //while (forceMultiplier > -5 || _a)
        //{
        //    _a = false;
        //     forceMultiplier = LiquidTrigger.GetForceMultiplier(transform.position.y);
        //    //MathF.Sqrt(MathF.Abs(transform.position.y - _triggerBounds))*5;//Liquid.GetFloatMultiplier(transform.position.y, _connectedLiquid);
        //    print(forceMultiplier);
        //    if (forceMultiplier > 0)
        //        _rigidbody2D.AddForce(-Physics.gravity * (forceMultiplier - _rigidbody2D.velocity.y * 0.01f));
            yield return new WaitForFixedUpdate();
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //_triggerCount++;
        //var s = collision.offset;
        //_triggerBounds = s.y;
        _rigidbody2D.gravityScale = 0.8f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //_triggerCount--;
    }


}
