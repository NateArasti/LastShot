using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Guest : MonoBehaviour
{
    private static readonly int SitOnChair = Animator.StringToHash("SitOnChair");
    private static readonly int SitToTable = Animator.StringToHash("SitToTable");
    private static readonly int OutlineEnabledProperty = Shader.PropertyToID("_Outline_Enabled");

    public enum SpotType
    {
        Chair,
        Table
    }

    public UnityEvent<Transform, Guest> OnDestroyEvent { get; } = new();

    [Header("Move Params")]
    [SerializeField] private float _secondsPerUnit = 5f;
    [SerializeField] private float _sitTime;
    [SerializeField] private float _delayBeforeGoingOutChair = 1f;
    [SerializeField] private float _delayBeforeGoingOutTable = 1.4f;

    [Header("Layer Params")] 
    [SerializeField] private int _normalLayer;
    [SerializeField] private int _chairSitLayer;
    [SerializeField] private int _tableSitLayer;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Vector3 _startPosition;
    private Transform _spotTransform;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void MoveToSpot(Transform spotTransform, SpotType spotType, bool moveRight)
    {
        _startPosition = transform.position;
        _spriteRenderer.flipX = moveRight;
        _spotTransform = spotTransform;
        _spriteRenderer.sortingOrder = _normalLayer;

        StartCoroutine(Move());

        IEnumerator Move()
        {
            yield return UnityExtensions.Wait(MoveToX(spotTransform.position.x));

            _spriteRenderer.sortingOrder = 
                spotType == SpotType.Table ? 
                    _tableSitLayer : 
                    _chairSitLayer;

            _animator.SetBool(spotType == SpotType.Table ? SitToTable : SitOnChair, true);

            yield return UnityExtensions.Wait(_sitTime);

            _animator.SetBool(spotType == SpotType.Table ? SitToTable : SitOnChair, false);
            if(spotType == SpotType.Table)
                _spriteRenderer.sortingOrder = _normalLayer;

            yield return UnityExtensions.Wait(spotType == SpotType.Table ? _delayBeforeGoingOutTable : _delayBeforeGoingOutChair);
            if (spotType == SpotType.Chair)
                _spriteRenderer.sortingOrder = _normalLayer;

            _spriteRenderer.flipX = !_spriteRenderer.flipX;
            yield return UnityExtensions.Wait(MoveToX(_startPosition.x));
            Destroy(gameObject);
        }
    }

    private float MoveToX(float newX)
    {
        var awaitTime = _secondsPerUnit * Mathf.Abs(newX - transform.position.x);
        transform
            .DOMoveX(newX, awaitTime)
            .SetEase(Ease.Linear);
        return awaitTime;
    }

    private void OnDestroy()
    {
        OnDestroyEvent.Invoke(_spotTransform, this);
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 1);
    }

    private void OnMouseExit()
    {
        _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 0);
    }
}