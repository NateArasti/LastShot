using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LiquidRenderer))]
public class StaticLiquid : MonoBehaviour
{
    private const float Spring = 0.02f; // �������� �����
    private const float Damping = 0.04f; // ��� ������ ��� ���� �����
    private const float Spread = 0.05f;
    
    [SerializeField] private int _edgeCount;
    [SerializeField] private LiquidTrigger _trigger;
    [SerializeField] private int _density;
    [SerializeField] private float _relaxation;
    [SerializeField] private float _topOffset = 2f;
    private readonly List<Vector2> _uvs = new();
    private float[] _accelerations;
    private float _baseHeight = 0.1f;
    private float _distanceBetweenVertices;
    private float _fixedTopGlassEdge;
    private LiquidRenderer _liquidRenderer;
    private Mesh _mesh;
    private Rect _rect;
    private List<int> _triangles;
    private float[] _velocities;
    private Vector3[] _vertices;
    private AnimationCurve _widthCurve;
    

    public void SpawnStartLiquid(RectTransform maskRect, AnimationCurve curve)
    {
        _widthCurve = curve;
        var corners = new Vector3[4];
        maskRect.GetWorldCorners(corners);
        var newRect = new Rect(corners[0], corners[2] - corners[0]);

        CreateMesh(newRect);
    }
    private void Awake()
    {
        _liquidRenderer = GetComponent<LiquidRenderer>();
    }

    private void Start()
    {
        UpdateTexture();
    }
    
    private void FixedUpdate()
    {
        CalculateTriggerBounds();
        CalculateVertices();
    }

    private void CreateMesh(Rect rect)
    {
        _triangles = new List<int>();
        _rect = rect;
        _fixedTopGlassEdge = rect.yMax - _topOffset;
        _mesh = new Mesh();
        _vertices = new Vector3[_edgeCount * 2 + 2];
        _velocities = new float[_edgeCount * 2 + 2];
        _accelerations = new float[_edgeCount * 2 + 2];

        CreateVertices(_edgeCount);

        _mesh.SetVertices(_vertices);
        _mesh.SetTriangles(_triangles, 0);
        _mesh.SetUVs(0, _uvs);
        GetComponent<MeshFilter>().mesh = _mesh;


        _trigger.OnHit.AddListener(Splash);

        StopAllCoroutines();
    }

    private void Splash(float x, float force = 0.3f, float volume = 0.001f)
    {
        //print(volume);
        if (_rect.height <= _fixedTopGlassEdge)
        {
            var a = Mathf.RoundToInt((x - _rect.x) / _distanceBetweenVertices);
            _velocities[2 * a + 1] -= force;

            StartCoroutine(FadeVolumeIncrease(_rect.height + volume * _baseHeight / _rect.width));
        }
    }

    private IEnumerator FadeVolumeIncrease(float targetHeight)
    {
        var delta = Mathf.Lerp(_rect.height, targetHeight, Time.deltaTime * 2.5f) - _rect.height;
        while (targetHeight > _rect.height)
        {
            for (var i = 1; i < _velocities.Length; i += 2)
                _vertices[i].y += delta;
            //print("!@#!@#@#@!");
            _rect.height += delta;
            yield return null;
        }
    }

    private void CreateVertices(int edgeCount)
    {
        _distanceBetweenVertices = _rect.width / edgeCount;
        _vertices[0] = new Vector3(_rect.x, _rect.y, 0);
        _uvs.Add(Vector2.zero);
        _rect.height = _rect.y + _baseHeight;
        var a = 1 / (float) edgeCount;

        for (var i = 1; i < edgeCount * 2 + 2; i++)
            if (i % 2 != 0)
            {
                _vertices[i] = new Vector3(_vertices[i - 1].x, _rect.height);
                _uvs.Add(new Vector2(i * a, 1));
            }
            else
            {
                _vertices[i] = new Vector3(_vertices[i - 1].x + _distanceBetweenVertices, _rect.y);
                _uvs.Add(new Vector2(i * a, 0));
            }


        for (var i = 0; i < edgeCount * 2; i++)
            _triangles.AddRange(new[]
            {
                i, i + 1, i + 2
            });
    }

    private void CalculateVertices()
    {
        for (var i = 1; i < _vertices.Length; i += 2)
        {
            var force = Spring * (_vertices[i].y - _rect.height) + _velocities[i] * Damping;
            _accelerations[i] = -force;
            _vertices[i].y += _velocities[i];
            _velocities[i] += _accelerations[i];
            if (i == 1 || i == _vertices.Length - 1)
                if (_velocities[i] != 0)
                    Splash(_vertices[i].x, _accelerations[i] * _relaxation);
        }

        var leftDeltas = new float[_vertices.Length];
        var rightDeltas = new float[_vertices.Length];

        for (var j = 0; j < _density; j++)
        {
            for (var i = 1; i < _vertices.Length; i += 2)
            {
                if (i > 1)
                {
                    leftDeltas[i] = Spread * (_vertices[i].y - _vertices[i - 2].y);
                    _velocities[i - 2] += leftDeltas[i];
                }

                if (i < _vertices.Length - 1)
                {
                    rightDeltas[i] = Spread * (_vertices[i].y - _vertices[i + 2].y);
                    _velocities[i + 2] += rightDeltas[i];
                }
            }

            for (var i = 1; i < _vertices.Length; i += 2)
            {
                if (i > 1)
                {
                    _vertices[i - 2].y += leftDeltas[i];
                    if (_vertices[i - 2].y <= _vertices[0].y)
                        _vertices[i - 2].y = _vertices[0].y;
                }

                if (i < _vertices.Length - 1)
                {
                    _vertices[i + 2].y += rightDeltas[i];
                    if (_vertices[i + 2].y < _vertices[0].y)
                        _vertices[i + 2].y = _vertices[0].y;
                }
            }
        }

        _mesh.vertices = _vertices;
    }

    private void CalculateTriggerBounds()
    {
        _baseHeight = _rect.height - _rect.y;
        _trigger.SetTriggerBounds(_widthCurve.Evaluate(_baseHeight)* _rect.width, _rect.height, _vertices[_vertices.Length / 2].x);
    }

    private void UpdateTexture()
    {
        _liquidRenderer.UpdateTexture();
    }


}