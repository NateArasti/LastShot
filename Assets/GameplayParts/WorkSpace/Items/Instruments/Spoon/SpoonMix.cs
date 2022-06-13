using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class SpoonMix : MonoBehaviour
{
    [Header("Liquid")]
    [SerializeField] private Image _liquidMask;
    [SerializeField] private Material _liquidMaskMaterial;
    [SerializeField] private float _liquidMixingSpeed = 1f;
    [Header("Circle")]
    [SerializeField] private RectTransform _rotationPivot;
    [SerializeField] private float _rotationDuration = 5f;
    [SerializeField] private Image _circleImage;
    [SerializeField] private Gradient _circleGradient;
    [SerializeField] private float _colorChangeDuration = 0.3f;
    [Header("Points")]
    [SerializeField] private float _confirmDuration = 0.2f;
    [SerializeField] private float _endScale = 1.2f;
    [SerializeField] private float _confirmAngle = 10f;
    [SerializeField] private RectTransform[] _points;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotationTween;
    private Vector3 _lastCirclePosition;
    private LiquidRenderer _liquidRenderer;
    private bool _started;
    private Color _startColor;
    private Color _targetColor;
    private static readonly int Speed = Shader.PropertyToID("_Speed");

    public void StartMixing(LiquidRenderer liquidRenderer)
    {
        _started = true;
        _liquidRenderer = liquidRenderer;
        _startColor = liquidRenderer.TopColor;
        _targetColor = liquidRenderer.GetCurrentAverageGradientColor();
        _liquidMask.color = _startColor;
        _rotationTween.Kill();
        _rotationTween = _rotationPivot
            .DOLocalRotate(
                new Vector3(0, 0, 360),
                _rotationDuration,
                RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    public void EndMixing()
    {
        var endColor = _liquidRenderer.TopColor;
        var distance = _startColor.GetDistanceTo(_targetColor);
        var passed = _startColor.GetDistanceTo(endColor);
        OrderCreationEvents.Instance.OrderActionsTracker.AddAction(new OrderAction.ShakerMixAction(false)
        {
            Intensity = passed / distance
        });
        _started = false; 
        _rotationTween.Kill();
        _rotationPivot.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if (!_started) return;
        var position = _circleImage.transform.position;
        if (_lastCirclePosition == Vector3.zero)
        {
            _lastCirclePosition = position;
            return;
        }
        var delta = (position - _lastCirclePosition).magnitude;
        _lastCirclePosition = position;
        _liquidRenderer.SmoothLiquid(delta * _liquidMixingSpeed * Time.deltaTime);
        _liquidMask.color = _liquidRenderer.TopColor;
    }

    public void CheckPoint(int index)
    {
        var angle = _rotationPivot.localEulerAngles.z % 360;
        switch (index)
        {
            case 0 when Mathf.Abs(angle - 90) < _confirmAngle:
            case 1 when Mathf.Abs(angle) < _confirmAngle || Mathf.Abs(angle - 360) < _confirmAngle:
            case 2 when Mathf.Abs(angle - 270) < _confirmAngle:
            case 3 when Mathf.Abs(angle - 180) < _confirmAngle:
                ConfirmPoint(_points[index]);
                break;
            default:
                ErrorCircle();
                break;
        }
    }


    private void ConfirmPoint(RectTransform pointTransform)
    {
        _rotationTween.timeScale += 0.1f;
        _liquidMaskMaterial.DOFloat(_rotationTween.timeScale, Speed, 0.5f);
        pointTransform.DOScale(_endScale * Vector3.one, _confirmDuration).SetLoops(2, LoopType.Yoyo);
    }

    private void ErrorCircle()
    {
        _rotationTween.timeScale -= 0.3f;
        _liquidMaskMaterial.DOFloat(_rotationTween.timeScale, Speed, 0.5f);
        if (_rotationTween.timeScale < 1)
        {
            _rotationTween.timeScale = 1;
        }
        _circleImage.DOGradientColor(_circleGradient, _colorChangeDuration).SetLoops(2, LoopType.Yoyo);
    }
}
