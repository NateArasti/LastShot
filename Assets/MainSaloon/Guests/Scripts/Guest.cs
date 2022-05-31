using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(RandomAnimations))]
public class Guest : MonoBehaviour
{
    private static readonly int SitOnChair = Animator.StringToHash("SitOnChair");
    private static readonly int SitToTable = Animator.StringToHash("SitToTable");
    private static readonly int OutlineEnabledProperty = Shader.PropertyToID("_Outline_Enabled");

    public enum SpotType
    {
        None,
        Chair,
        Table
    }

    public UnityEvent<Transform, Guest> OnDestroyEvent { get; } = new();
    public bool Sitting { get; private set; }

    private bool MouseEnabled => Sitting && GameStateManager.CurrentState == GameStateManager.State.Saloon;

    [Header("Move Params")]
    [SerializeField] private float _secondsPerUnit = 5f;
    [SerializeField] private float _sitTime;
    [SerializeField] private float _delayBeforeGoingOutChair = 1f;
    [SerializeField] private float _delayBeforeGoingOutTable = 1.4f;

    [Header("Layer Params")] 
    [SerializeField] private int _normalLayer;
    [SerializeField] private int _chairSitLayer;
    [SerializeField] private int _tableSitLayer;

    [Header("Animations")]
    [SerializeField] private RandomAnimations _chairRandomAnimations;
    [SerializeField] private RandomAnimations _tableRandomAnimations;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Vector3 _startPosition;
    private Transform _spotTransform;
    private SpotType _currentSpotType;

    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckRandomAnimation();
    }

    private void CheckRandomAnimation()
    {
        switch (_currentSpotType)
        {
            case SpotType.None:
                _chairRandomAnimations.enabled = false;
                _tableRandomAnimations.enabled = false;
                break;
            case SpotType.Chair:
                _chairRandomAnimations.enabled = Sitting;
                _tableRandomAnimations.enabled = false;
                break;
            case SpotType.Table:
                _chairRandomAnimations.enabled = false;
                _tableRandomAnimations.enabled = Sitting;
                break;
        }
    }

    public void MoveToSpot(Transform spotTransform, SpotType spotType, bool moveRight)
    {
        _startPosition = transform.position;
        _spriteRenderer.flipX = moveRight;
        _spotTransform = spotTransform;
        _currentSpotType = spotType;
        _spriteRenderer.sortingOrder = _normalLayer;

        _moveCoroutine = StartCoroutine(Move());

        IEnumerator Move()
        {
            yield return UnityExtensions.Wait(MoveToX(spotTransform.position.x));

            _spriteRenderer.sortingOrder = 
                spotType == SpotType.Table ? 
                    _tableSitLayer : 
                    _chairSitLayer;

            _animator.SetBool(spotType == SpotType.Table ? SitToTable : SitOnChair, true);
            Sitting = true;
            yield return UnityExtensions.Wait(_sitTime);
            yield return StartCoroutine(Exit());
        }
    }

    private IEnumerator Exit()
    {
        Sitting = false;
        _animator.SetBool(_currentSpotType == SpotType.Table ? SitToTable : SitOnChair, false);
        if (_currentSpotType == SpotType.Table)
            _spriteRenderer.sortingOrder = _normalLayer;

        yield return UnityExtensions.Wait(_currentSpotType == SpotType.Table ? 
            _delayBeforeGoingOutTable : _delayBeforeGoingOutChair);
        if (_currentSpotType == SpotType.Chair)
            _spriteRenderer.sortingOrder = _normalLayer;

        _spriteRenderer.flipX = !_spriteRenderer.flipX;
        yield return UnityExtensions.Wait(MoveToX(_startPosition.x));
        Destroy(gameObject);
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
        if (!MouseEnabled) return;
        _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 1);
    }

    private void OnMouseExit()
    {
        if (!MouseEnabled) return;
        _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 0);
    }

    private void OnMouseDown()
    {
        if (!MouseEnabled) return;
        if(_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        StartCoroutine(Exit());
        _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 0);
    }
}