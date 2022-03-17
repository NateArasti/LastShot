using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CameraScrollParams))]
public class CameraScrollRect : MonoBehaviour
{
    private CameraScrollParams _params;

    private Camera _camera;
    private Transform _mainCameraTransform;
    private Coroutine _frictionCoroutine;
    private float _currentSpeed;
    private float _cameraWidth;
    private float _cameraHeight;
    private float _direction = 1;
    private float _previousDirection = 1;

    private float LeftEdge => _mainCameraTransform.position.x - _params.WidthPercent * _cameraWidth;
    private float RightEdge => _mainCameraTransform.position.x + _params.WidthPercent * _cameraWidth;
    private float TopEdge => _mainCameraTransform.position.y + _params.HeightPercent * _cameraHeight;
    private float BottomEdge => _mainCameraTransform.position.y - _params.HeightPercent * _cameraHeight;

    private void Start()
    {
        _params = GetComponent<CameraScrollParams>();

        _camera = _params.Camera;
        Debug.Assert(_camera != null, "Camera null");
        _mainCameraTransform = _camera.transform;
        _cameraWidth = _camera.orthographicSize * _camera.pixelWidth / _camera.pixelHeight;
        _cameraHeight = _camera.orthographicSize;
    }

    private void LateUpdate()
    {
        if (CheckIfCursorOnEdge(out var speedApplier))
        {
            if(_frictionCoroutine != null)
            {
                StopCoroutine(_frictionCoroutine);
                _frictionCoroutine = null;
                _currentSpeed = Mathf.Approximately(_direction, _previousDirection) ? 0 : -_params.Friction * _currentSpeed;
            }
            //_currentSpeed += _params.ScrollSpeed * speedApplier * Time.deltaTime;
            _currentSpeed += _params.ScrollSpeed * Time.deltaTime;
            var cameraPosition = _mainCameraTransform.position;
            cameraPosition.x += _direction * _currentSpeed;
            _mainCameraTransform.position = cameraPosition;
            _previousDirection = _direction;
        }
        else if (_currentSpeed != 0 && _frictionCoroutine == null) 
            _frictionCoroutine = StartCoroutine(StartFriction());
        _mainCameraTransform.position = ClampPosition(_mainCameraTransform.position);
    }

    private bool CheckIfCursorOnEdge(out float speedApplier)
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        speedApplier = 0;
        if (TopEdge < mousePosition.y || mousePosition.y < BottomEdge)
            return false;

        var rightEdge = RightEdge;
        var leftEdge = LeftEdge;

        if (mousePosition.x < leftEdge)
        {
            speedApplier = (leftEdge - mousePosition.x) / (_mainCameraTransform.position.x + _cameraWidth - rightEdge);
            _direction = -1;
        }
        else if (mousePosition.x > rightEdge)
        {
            speedApplier = (mousePosition.x - rightEdge) / (_mainCameraTransform.position.x + _cameraWidth - rightEdge);
            _direction = 1;
        }
        else 
            return false;

        return true;
    }

    private IEnumerator StartFriction()
    {
        while (_currentSpeed > 0)
        {
            _currentSpeed -= _params.Friction * Time.deltaTime;
            if (_currentSpeed < 0)
                _currentSpeed = 0;
            var cameraPosition = _mainCameraTransform.position;
            cameraPosition.x += _direction * _currentSpeed;
            _mainCameraTransform.position = ClampPosition(cameraPosition);
            yield return null;
        }

        _currentSpeed = 0;
        _frictionCoroutine = null;
    }

    private Vector3 ClampPosition(Vector3 newPosition)
    {
        if (Mathf.Approximately(newPosition.x, _params.LeftBorder.position.x + _cameraWidth) ||
            Mathf.Approximately(newPosition.x, _params.RightBorder.position.x - _cameraWidth))
        {
            _currentSpeed = 0;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, _params.LeftBorder.position.x + _cameraWidth,
            _params.RightBorder.position.x - _cameraWidth);
        return newPosition;
    }
}
