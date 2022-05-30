using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CameraScrollUI : MonoBehaviour
{
    [SerializeField] private float _maxAlpha = 0.5f;
    [SerializeField] private Image _leftImage;
    [SerializeField] private Image _rightImage;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public void SetUI(float width, float height, UnityEvent<float> mousePositionChanged, Camera refCamera)
    {
        _canvas.worldCamera = refCamera;
        var newSizeDelta = new Vector2(width, -height) * 0.5f;
        print(newSizeDelta);
        _leftImage.rectTransform.sizeDelta = newSizeDelta;
        _rightImage.rectTransform.sizeDelta = newSizeDelta;
        mousePositionChanged.AddListener(HandleMousePosition);
    }

    private void HandleMousePosition(float position)
    {
        if (position < 0)
        {
            _rightImage.Fade(0);
            _leftImage.Fade(Mathf.Clamp01(Mathf.Lerp(0, 1, Mathf.Abs(position))) * _maxAlpha);
        }
        else
        {
            _leftImage.Fade(0);
            _rightImage.Fade(Mathf.Clamp01(Mathf.Lerp(0, 1, position)) * _maxAlpha);
        }
    }
}
