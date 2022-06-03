using UnityEngine;

public class LiquidRenderer : MonoBehaviour
{
    private static readonly int GradientTexture = Shader.PropertyToID("_GradientTexture");
    [SerializeField] private float _gradientResolution;
    [SerializeField] private Gradient _previousGradient;
    private float[] _colorHeight = new float[2];
    private GradientColorKey[] _colorKeys;
    private bool _firstGradUpdt = true;

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
        var material = GetComponent<MeshRenderer>().material;
        material.SetTexture(GradientTexture, gradientTexture);
    }

    public void UpdateGradient(Color color, float liquidHeight)
    {
        var gck = new GradientColorKey[_previousGradient.colorKeys.Length];
        var gak = new GradientAlphaKey[_previousGradient.alphaKeys.Length];

        if (_firstGradUpdt)
        {
            for (var i = 0; i < gck.Length; i++)
            {
                gck[i].color = color;
                gck[i].time = _previousGradient.colorKeys[i].time;
                gak[i].alpha = _previousGradient.alphaKeys[i].alpha;
                gak[i].time = _previousGradient.alphaKeys[i].time;
            }

            _colorHeight[0] = 0f;
            _previousGradient.SetKeys(gck, gak);
            _firstGradUpdt = false;
            return;
        }

        if (_previousGradient.colorKeys[^1].color == color)
        {
            _colorHeight[_previousGradient.colorKeys.Length - 1] = liquidHeight;
            var sameColors = new GradientColorKey[_previousGradient.colorKeys.Length];
            for (var i = 0; i < sameColors.Length; i++) sameColors[i] = _previousGradient.colorKeys[i];
            for (var i = 0; i < _colorHeight.Length; i++) sameColors[i].time = _colorHeight[i] / liquidHeight;
            _previousGradient.SetKeys(sameColors, _previousGradient.alphaKeys);
            return;
        }

        _colorKeys = new GradientColorKey[_previousGradient.colorKeys.Length + 1];

        var newColorHeight = new float[_previousGradient.colorKeys.Length + 1];
        for (var i = 0; i < _colorHeight.Length; i++)
            newColorHeight[i] = _colorHeight[i];
        newColorHeight[^1] = liquidHeight;

        for (var i = 0; i < _colorKeys.Length; i++)
        {
            _colorKeys[i].color = i == _colorKeys.Length - 1 ? color : _previousGradient.colorKeys[i].color;
            _colorKeys[i].time = newColorHeight[i] / liquidHeight;
        }

        _colorHeight = new float[newColorHeight.Length];
        for (var i = 0; i < newColorHeight.Length; i++) _colorHeight[i] = newColorHeight[i];

        _previousGradient.SetKeys(_colorKeys, _previousGradient.alphaKeys);
    }
}