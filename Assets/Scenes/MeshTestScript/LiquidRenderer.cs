using UnityEngine;

public class LiquidRenderer : MonoBehaviour
{
    [SerializeField] private float _gradientResolution;
    [SerializeField] private Gradient _gradient;
    private static readonly int GradientTexture = Shader.PropertyToID("_GradientTexture");

    private Texture2D GenerateTexture(bool makeNoLongerReadable = false)
    {
        var tex = new Texture2D(1, (int) _gradientResolution, TextureFormat.ARGB32, false, true);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        tex.anisoLevel = 1;

        var colors = new Color[(int) _gradientResolution];
        float div = (int) _gradientResolution;
        for (var i = 0; i < (int) _gradientResolution; ++i)
        {
            var t = i / div;
            colors[i] = _gradient.Evaluate(t);
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


    //[ContextMenu("ConvertToPng")]
    //public void ConvertToPNG()
    //{
    //    //print(Application.dataPath);
    //    var tex = GenerateTexture();

    //    byte[] bytes = tex.EncodeToPNG();
    //    File.WriteAllBytes(Application.dataPath + "/GradientTex.png", bytes);
    //    DestroyImmediate(tex);
    //}
}