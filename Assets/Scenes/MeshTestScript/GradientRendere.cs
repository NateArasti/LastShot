using System.IO;
using UnityEngine;

public class GradientRendere : MonoBehaviour
{
    [SerializeField] private float _GradientResolution;
    [SerializeField] private Gradient _Gradient;

    private void Awake()
    {
    }

    public Texture2D GenerateTexture(bool makeNoLongerReadable = false)
    {
        var tex = new Texture2D(1, (int) _GradientResolution, TextureFormat.ARGB32, false, true);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;
        tex.anisoLevel = 1;
        var colors = new Color[(int) _GradientResolution];
        float div = (int) _GradientResolution;
        for (var i = 0; i < (int) _GradientResolution; ++i)
        {
            var t = i / div;
            colors[i] = _Gradient.Evaluate(t);
        }

        tex.SetPixels(colors);
        tex.Apply(false, makeNoLongerReadable);


        return tex;
    }

    [ContextMenu("ConvertToPng")]
    public void ConvertToPNG()
    {
        print(Application.dataPath);
        //var tex = GenerateTexture();

        //var bytes = tex.EncodeToPNG();
        //File.WriteAllBytes(Application.dataPath + "/GradientTex.png", bytes);
        //DestroyImmediate(tex);
    }
}