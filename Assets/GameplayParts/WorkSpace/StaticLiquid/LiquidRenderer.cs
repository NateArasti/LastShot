using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LiquidRenderer : MonoBehaviour
{
    private static readonly int GradientTexture = Shader.PropertyToID("_GradientTexture");
    [SerializeField] private float _gradientResolution;
    [SerializeField] private Gradient _previousGradient;
    private float[] _colorHeight = new float[2];
    private GradientColorKey[] _colorKeys;
    private GradientAlphaKey[] _alphaKeys;
    private bool _firstGradientUpdate = true;
    private MeshRenderer _renderer;
    private Material _material;

    public Color TopColor => _previousGradient.Evaluate(1).GetRGB();
    public Gradient CurrentGradient => _previousGradient;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
    }

    public static (GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys) 
        GetGradientSmoothing(float step, Gradient gradient)
    {
        var length = gradient.colorKeys.Length;
        var colorKeys = gradient.colorKeys;
        var alphaKeys = gradient.alphaKeys;
        var averageColor = new Vector4(0, 0, 0, 0);
        for (var i = 0; i < length; i++)
        {
            var currentColor = colorKeys[i].color;
            currentColor.a = alphaKeys[i].alpha;
            averageColor += new Vector4(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        }

        averageColor /= length;

        var endColor = new Color(averageColor.x, averageColor.y, averageColor.z, averageColor.w);
        for (var i = 0; i < length; i++)
        {
            colorKeys[i].color = Color.Lerp(colorKeys[i].color, endColor.GetRGB(), step);
            alphaKeys[i].alpha = Mathf.Lerp(alphaKeys[i].alpha, endColor.a, step);
        }

        return (colorKeys, alphaKeys);
    }

    public Color GetCurrentAverageGradientColor()
    {
        var length = _previousGradient.colorKeys.Length;
        var colorKeys = _previousGradient.colorKeys;
        var alphaKeys = _previousGradient.alphaKeys;
        var averageColor = new Vector4(0, 0, 0, 0);
        for (var i = 0; i < length; i++)
        {
            var currentColor = colorKeys[i].color;
            currentColor.a = alphaKeys[i].alpha;
            averageColor += new Vector4(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        }

        averageColor /= length;

        return new Color(averageColor.x, averageColor.y, averageColor.z, averageColor.w);
    }

    public void SmoothLiquid(float step)
    {
        var (colorKeys, alphaKeys) = GetGradientSmoothing(step, _previousGradient);
        _previousGradient.SetKeys(colorKeys, alphaKeys);
        UpdateTexture();
    }

    private Texture2D GenerateTexture(bool makeNoLongerReadable = false)
    {
        var tex = new Texture2D(1, (int) _gradientResolution, TextureFormat.ARGB32, false, true)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear,
            anisoLevel = 1
        };

        var colors = new Color[(int) _gradientResolution];
        float div = (int) _gradientResolution;
        for (var i = 0; i < (int) _gradientResolution; ++i)
        {
            var t = i / div;
            colors[i] = _previousGradient.Evaluate(t);
        }

        tex.SetPixels(colors);
        tex.Apply(false, makeNoLongerReadable);


        return tex;
    }

    public void UpdateTexture()
    {
        var gradientTexture = GenerateTexture();
        _material.SetTexture(GradientTexture, gradientTexture);
    }

    public void UpdateGradient(Color color, float liquidHeight)
    {
        if (_firstGradientUpdate)
        {
            var gck = new GradientColorKey[2];
            var gak = new GradientAlphaKey[2];
            for (var i = 0; i < gck.Length; i++)
            {
                gck[i].color = color;
                gck[i].time = _previousGradient.colorKeys[i].time;
                gak[i].alpha = color.a;
                gak[i].time = _previousGradient.alphaKeys[i].time;
            }

            _colorHeight[0] = 0f;
            _previousGradient.SetKeys(gck, gak);
            _firstGradientUpdate = false;
            return;
        }

        if (_previousGradient.colorKeys[^1].color.GetRGB() == color.GetRGB())
        {
            var length = _previousGradient.colorKeys.Length;
            _colorHeight[length - 1] = liquidHeight;
            var sameColors = new GradientColorKey[length];
            var sameAlphas = new GradientAlphaKey[length];
            for (var i = 0; i < length; i++)
            {
                sameColors[i] = _previousGradient.colorKeys[i];
                sameAlphas[i] = _previousGradient.alphaKeys[i];
            }
            for (var i = 0; i < _colorHeight.Length; i++)
            {
                var position = _colorHeight[i] / liquidHeight;
                sameColors[i].time = position;
                sameAlphas[i].time = position;
            }
            _previousGradient.SetKeys(sameColors, sameAlphas);
            return;
        }

        var keyLength = _previousGradient.colorKeys.Length + 1;
        _colorKeys = new GradientColorKey[keyLength];
        _alphaKeys = new GradientAlphaKey[keyLength];

        var newColorHeight = new float[keyLength];
        for (var i = 0; i < keyLength - 1; i++)
            newColorHeight[i] = _colorHeight[i];
        newColorHeight[^1] = liquidHeight;
        for (var i = 0; i < keyLength; i++)
        {
            var position = newColorHeight[i] / liquidHeight;

            _colorKeys[i].color = i == _colorKeys.Length - 1 ? color.GetRGB() : _previousGradient.colorKeys[i].color;
            _colorKeys[i].time = position;

            _alphaKeys[i].alpha = i == _colorKeys.Length - 1 ? color.a : _previousGradient.alphaKeys[i].alpha;
            _alphaKeys[i].time = position;
        }

        _colorHeight = new float[keyLength];
        for (var i = 0; i < keyLength; i++) _colorHeight[i] = newColorHeight[i];

        _previousGradient.SetKeys(_colorKeys, _alphaKeys);
    }
}