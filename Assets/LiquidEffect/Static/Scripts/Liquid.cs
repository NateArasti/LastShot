using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Algorithm based on this https://gamedevelopment.tutsplus.com/tutorials/creating-dynamic-2d-water-effects-in-unity--gamedev-14143

[RequireComponent(typeof(LineRenderer))]
public class Liquid : MonoBehaviour
{
    private static 
        Dictionary<Liquid, (List<Liquid> connectedLiquids, List<Liquid> staticLiquids, float maxHeight, Color averageColor)> SpawnedGlassLiquids =
        new Dictionary<Liquid, (List<Liquid> connectedLiquids, List<Liquid> staticLiquids, float maxHeight, Color averageColor)>();

    private const float SpringConstant = 0.02f;
    private const float Damping = 0.04f;
    private const float Spread = 0.05f;
    private const float Z = 1f;
    private const float StartHeight = 0.005f;
    private const float StirStep = 2f;

    public int Index { get; private set; }
    public Liquid ParentLiquid { get; private set; }
    public readonly UnityEvent OnDestroyEvent = new UnityEvent();

    [SerializeField] private Camera _staticLiquidCamera;
    [Header("Prefabs")]
    [SerializeField] private GameObject _emptyLiquidSpawnPrefab;
    [SerializeField] private GameObject _liquidMesh;
    [SerializeField] private GameObject _splashEffect;
    [Header("Params")]
    [SerializeField] private int _edgePerUnit = 5;

    private bool _fixed = false;
    private Transform _trashStorage;

    private Rect _liquidRect;
    private LineRenderer _liquidTopLine;
    private (Color mainColor, Color outlineColor) _colors = (Color.white, Color.red);
    private (Color mainColor, Color outlineColor) _startColors = (Color.white, Color.red);

    private Vector2[] _verticesPositions;
    private float[] _velocities;
    private float[] _accelerations;

    private GameObject[] _meshObjects;
    private Mesh[] _meshes;
    private GameObject[] _colliders;
    private int _edgeCount;

    private float baseHeight => _liquidRect.y + _liquidRect.height;

    public static float GetFloatMultiplier(float yPosition, Liquid parentLiquid)
    {
        var liquids = SpawnedGlassLiquids[parentLiquid].connectedLiquids;
        var height = liquids[liquids.Count - 1].baseHeight;
        return (yPosition - height) / height;
    }

    private static (float pos, float scale) GetColliderYParams(Liquid parentLiquid)
    {
        var liquids = SpawnedGlassLiquids[parentLiquid].connectedLiquids;
        var bottom = liquids[0]._liquidRect.y;
        var top = liquids[liquids.Count - 1].baseHeight;
        var scale = Mathf.Max(top - bottom, 1);
        return (top - 0.5f * scale, scale);
    }

    public static void StirAllConnectedLiquids(float intensity, Liquid parentLiquid)
    {
        var liquids = new Liquid[SpawnedGlassLiquids[parentLiquid].connectedLiquids.Count + SpawnedGlassLiquids[parentLiquid].staticLiquids.Count];
        SpawnedGlassLiquids[parentLiquid].connectedLiquids.CopyTo(liquids);
        SpawnedGlassLiquids[parentLiquid].staticLiquids.CopyTo(liquids, SpawnedGlassLiquids[parentLiquid].connectedLiquids.Count);
        var averageColor = Vector4.zero;
        var totalMass = 0f;
        for (var i = 1; i < liquids.Length; ++i)
        {
            var color = liquids[i]._startColors.mainColor;
            Color.RGBToHSV(color, out var H, out var S, out var V);
            averageColor += liquids[i]._liquidRect.height * new Vector4(H, S, V, color.a);
            totalMass += liquids[i]._liquidRect.height;
        }
        averageColor /= totalMass;
        var newColor = Color.HSVToRGB(averageColor.x, averageColor.y, averageColor.z);
        newColor.a = averageColor.w;
        for (var i = 0; i < liquids.Length; ++i)
        {
            var oldColor = liquids[i]._colors.mainColor;
            liquids[i].SetColor(
                (Color.Lerp(oldColor, newColor, StirStep * intensity * Time.deltaTime), 
                Color.white));
        }
    }

    public bool TryGetColorAfterStir(out (Color mainColor, Color outlineColor) colors)
    {
        var liquids = SpawnedGlassLiquids[ParentLiquid].connectedLiquids;
        var firstColor = liquids[0]._colors.mainColor;
        var lastColor = liquids[liquids.Count - 1]._colors.mainColor;
        var delta = 0.015f;
        print(Mathf.Abs(firstColor.r - lastColor.r));
        print(Mathf.Abs(firstColor.g - lastColor.g));
        print(Mathf.Abs(firstColor.b - lastColor.b));
        print(Mathf.Abs(firstColor.a - lastColor.a));
        if (Mathf.Abs(firstColor.r - lastColor.r) < delta &&
            Mathf.Abs(firstColor.g - lastColor.g) < delta &&
            Mathf.Abs(firstColor.b - lastColor.b) < delta &&
            Mathf.Abs(firstColor.a - lastColor.a) < delta)
        {
            colors = _colors;
            return true;
        }
        colors = (Color.white, Color.white);
        return false;
    }

    private void OnDestroy()
    {
        OnDestroyEvent.Invoke();
    }

    private void SetColor((Color mainColor, Color outlineColor) colors)
    {
        _liquidTopLine.startColor = colors.outlineColor;
        _colors = colors;
        foreach (var mesh in _meshObjects)
        {
            mesh.GetComponent<MeshRenderer>().material.color = colors.mainColor;
        }
    }

    public void Splash(Vector2 position, float velocity, float mass)
    {
        if (_fixed) return;

        var xPosition = position.x;

        _liquidRect.height = Mathf.Min(mass / _liquidRect.width + _liquidRect.height, SpawnedGlassLiquids[ParentLiquid].maxHeight);

        var length = _verticesPositions.Length;

        if (!(xPosition >= _verticesPositions[0].x) ||
            !(xPosition <= _verticesPositions[length - 1].x)) return;

        xPosition -= _verticesPositions[0].x;
        
        var index = Mathf.RoundToInt(
            (length - 1) * 
            (xPosition / (_verticesPositions[length - 1].x - _verticesPositions[0].x)));
        
        _velocities[index] += velocity;

        //Instantiate(_splashEffect, position, Quaternion.identity)
        //    .GetComponent<SplashEffect>()
        //    .Splash(_liquidColor);
    }

    public void CreateNewLiquidAbove(int index, (Color mainColor, Color outlineColor) colors)
    {
        if (_fixed) return;
        FixLiquid();
        var newRect = new Rect(_liquidRect.x, baseHeight, _liquidRect.width, StartHeight);
        var liquid = Instantiate(_emptyLiquidSpawnPrefab, ParentLiquid.transform).GetComponent<Liquid>();
        liquid._emptyLiquidSpawnPrefab = _emptyLiquidSpawnPrefab;
        liquid._colors = colors;
        liquid._startColors = colors;
        liquid.Index = index;
        liquid.ParentLiquid = ParentLiquid;
        SpawnedGlassLiquids[liquid.ParentLiquid].connectedLiquids.Add(liquid);
        liquid.SpawnLiquid(newRect);
        if(ParentLiquid == this)
        {
            SetColor(colors);
        }
    }

    public void SpawnStaticLiquid((Color mainColor, Color outlineColor) colors, float height, Liquid parentLiquid)
    {
        var liquids = SpawnedGlassLiquids[parentLiquid].connectedLiquids;
        var newRect = new Rect(liquids[0]._liquidRect.x, liquids[0]._liquidRect.y,
            liquids[0]._liquidRect.width, height);
        var liquid = Instantiate(_emptyLiquidSpawnPrefab, ParentLiquid.transform).GetComponent<Liquid>();
        liquid._colors = colors;
        liquid._startColors = colors;
        liquid.ParentLiquid = parentLiquid;
        SpawnedGlassLiquids[parentLiquid].staticLiquids.Add(liquid);
        liquid.SpawnLiquid(newRect);
        liquid.FixLiquid();
        liquid.transform.position += Vector3.back;
    }

    public Liquid SpawnStartLiquid(RectTransform maskRect)
    {
        var zeroPoint = _staticLiquidCamera.ScreenToWorldPoint(Vector3.zero);
        var widthHeight = _staticLiquidCamera.ScreenToWorldPoint(new Vector3(maskRect.rect.width, maskRect.rect.height)) - zeroPoint;
        var newRect = new Rect(
            maskRect.position.x - maskRect.anchorMax.x * widthHeight.x,
            maskRect.position.y - maskRect.anchorMax.y * widthHeight.y,
            widthHeight.x, StartHeight);
        SpawnedGlassLiquids.Add(this, (new List<Liquid>() { this}, new List<Liquid>(), widthHeight.y, Color.black));
        ParentLiquid = this;
        SpawnLiquid(newRect);
        return this;
    }

    private void Awake()
    {
        _liquidTopLine = GetComponent<LineRenderer>();
        _trashStorage = new GameObject { name = "TrashStorage" }.transform;
        _trashStorage.parent = transform;
    }

    private void FixLiquid()
    {
        _fixed = true;
        foreach (var collider in _colliders)
        {
            Destroy(collider);
        }
        _liquidTopLine.enabled = false;
        this.enabled = false;
    }

    private void SpawnLiquid(Rect newRect)
    {
        _liquidRect = newRect;
        _liquidTopLine.startColor = _colors.outlineColor;

        _edgeCount = Mathf.RoundToInt(_liquidRect.width) * _edgePerUnit;
        var nodeCount = _edgeCount + 1;

        _liquidTopLine.positionCount = nodeCount;
        _verticesPositions = new Vector2[nodeCount];
        _velocities = new float[nodeCount];
        _accelerations = new float[nodeCount];

        _meshObjects = new GameObject[_edgeCount];
        _meshes = new Mesh[_edgeCount];
        _colliders = new GameObject[_edgeCount];

        for (var i = 0; i < nodeCount; i++)
        {
            _verticesPositions[i] = new Vector2(_liquidRect.x + i * _liquidRect.width / _edgeCount, baseHeight);
            _liquidTopLine.SetPosition(i, _verticesPositions[i]);
            _accelerations[i] = 0;
            _velocities[i] = 0;
        }
        for (var i = 0; i < _edgeCount; i++)
        {
            CreateMesh(i);
            CreateCollider(i);
        }
    }

    private void CreateCollider(int index)
    {
        _colliders[index] = new GameObject { name = "Trigger" };
        var boxCollider2D = _colliders[index].AddComponent<BoxCollider2D>();
        _colliders[index].transform.parent = transform;

        var yParams = GetColliderYParams(ParentLiquid);
        _colliders[index].transform.position = new Vector3(
            _liquidRect.x + _liquidRect.width * (index + 0.5f) / _edgeCount,
            yParams.pos, 0);
        _colliders[index].transform.localScale = new Vector3(_liquidRect.width / _edgeCount, yParams.scale, 1);
        boxCollider2D.isTrigger = true;

        _colliders[index].AddComponent<LiquidTouchDetector>();
    }

    private void CreateMesh(int index)
    {
        _meshes[index] = new Mesh();

        var vertices = GetVertices(index);

        var uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 1);
        uvs[1] = new Vector2(1, 1);
        uvs[2] = new Vector2(0, 0);
        uvs[3] = new Vector2(1, 0);

        _meshes[index].vertices = vertices;
        _meshes[index].uv = uvs;
        _meshes[index].triangles = new[] { 0, 1, 3, 3, 2, 0 };
        
        _meshObjects[index] = Instantiate(_liquidMesh, Vector3.zero, Quaternion.identity, _trashStorage);
        _meshObjects[index].GetComponent<MeshFilter>().mesh = _meshes[index];
        _meshObjects[index].GetComponent<MeshRenderer>().material.color = _colors.mainColor;
    }

    private Vector3[] GetVertices(int index)
    {
        var vertices = new Vector3[4];
        vertices[0] = new Vector3(_verticesPositions[index].x, _verticesPositions[index].y, Z);
        vertices[1] = new Vector3(_verticesPositions[index + 1].x, _verticesPositions[index + 1].y, Z);
        vertices[2] = new Vector3(_verticesPositions[index].x, _liquidRect.y, Z);
        vertices[3] = new Vector3(_verticesPositions[index + 1].x, _liquidRect.y, Z);
        return vertices;
    }

    private void UpdateVertices()
    {
        for (var i = 0; i < _meshes.Length; i++) _meshes[i].vertices = GetVertices(i);
    }

    private void UpdateColliders()
    {
        for (var i = 0; i < _meshes.Length; i++)
        {
            var yParams = GetColliderYParams(ParentLiquid);
            var oldPos = _colliders[i].transform.position;
            _colliders[i].transform.position = new Vector3(oldPos.x, yParams.pos, oldPos.z);
            var oldScale = _colliders[i].transform.localScale;
            _colliders[i].transform.localScale = new Vector3(oldScale.x, yParams.scale, 1);
        }
    }

    private void FixedUpdate()
    {

        for (var i = 0; i < _verticesPositions.Length; i++)
        {
            var force = SpringConstant * (_verticesPositions[i].y - baseHeight) + _velocities[i] * Damping;
            _accelerations[i] = -force;
            _verticesPositions[i].y += _velocities[i];
            _velocities[i] += _accelerations[i];
            _liquidTopLine.SetPosition(i, new Vector3(_verticesPositions[i].x, _verticesPositions[i].y, Z));
        }

        var leftDeltas = new float[_verticesPositions.Length];
        var rightDeltas = new float[_verticesPositions.Length];

        for (var j = 0; j < 8; j++)
        {
            for (var i = 1; i < _verticesPositions.Length; i++)
            {
                if (i > 0)
                {
                    leftDeltas[i] = Spread * (_verticesPositions[i].y - _verticesPositions[i - 1].y);
                    _velocities[i - 1] += leftDeltas[i];
                }
                if (i < _verticesPositions.Length - 1)
                {
                    rightDeltas[i] = Spread * (_verticesPositions[i].y - _verticesPositions[i + 1].y);
                    _velocities[i + 1] += rightDeltas[i];
                }
            }

            for (var i = 0; i < _verticesPositions.Length; i++)
            {
                if (i > 0)
                    _verticesPositions[i - 1].y += leftDeltas[i];
                if (i < _verticesPositions.Length - 1)
                    _verticesPositions[i + 1].y += rightDeltas[i];
            }
        }

        UpdateVertices();
        UpdateColliders();
    }
}