using UnityEngine;

[RequireComponent(typeof(CameraScrollParams))]
public class CameraScrollRect : MonoBehaviour
{
    private enum Direction
    {
        Right = 1,
        Left = -1
    }

    private CameraScrollParams _params;

    private Camera _camera;
    private Transform _mainCameraTransform;
    private float _currentSpeed;
    private float _cameraWidth;
    private float _cameraHeight;
    private Direction _currentDirection;

    private float LeftEdge => _mainCameraTransform.position.x - _params.WidthPercent * _cameraWidth;
    private float RightEdge => _mainCameraTransform.position.x + _params.WidthPercent * _cameraWidth;
    private float TopEdge => _mainCameraTransform.position.y + _params.HeightPercent * _cameraHeight;
    private float BottomEdge => _mainCameraTransform.position.y - _params.HeightPercent * _cameraHeight;

    private void Awake()
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
        if (CheckIfCursorOnEdge())
        {
            _currentSpeed += (int) _currentDirection * _params.ScrollSpeed * Time.deltaTime;
        }
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0, Time.deltaTime * _params.Friction);
        
        var position = _mainCameraTransform.position;
        position.x += _currentSpeed;
        position = ClampPosition(position);
        _mainCameraTransform.position = position;
    }

    private bool CheckIfCursorOnEdge()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (TopEdge < mousePosition.y || mousePosition.y < BottomEdge)
            return false;

        if (mousePosition.x < LeftEdge)
            _currentDirection = Direction.Left;
        else if (mousePosition.x > RightEdge)
            _currentDirection = Direction.Right;
        else
            return false;

        return true;
    }

    private Vector3 ClampPosition(Vector3 newPosition)
    {
        newPosition.x = Mathf.Clamp(newPosition.x, 
            _params.LeftBorder.position.x + _cameraWidth,
            _params.RightBorder.position.x - _cameraWidth);

        if (Mathf.Approximately(newPosition.x, _params.LeftBorder.position.x + _cameraWidth) ||
            Mathf.Approximately(newPosition.x, _params.RightBorder.position.x - _cameraWidth))
        {
            _currentSpeed = 0;
        }

        return newPosition;
    }
}
