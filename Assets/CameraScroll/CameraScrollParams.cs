using UnityEngine;

public class CameraScrollParams : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _scrollSpeed = 0.25f;
    [SerializeField] private float _friction = 0.5f;
    [Range(0, 1)] [SerializeField] private float _widthPercent = 0.85f;
    [Range(0, 1)] [SerializeField] private float _heightPercent = 0.7f;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;

    public Camera Camera => _camera;
    public float ScrollSpeed => _scrollSpeed;
    public float Friction => _friction;
    public float WidthPercent => _widthPercent;
    public float HeightPercent => _heightPercent;
    public Transform LeftBorder => _leftBorder;
    public Transform RightBorder => _rightBorder;
}
