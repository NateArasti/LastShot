using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SplashEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Splash(Color splashColor)
    {
        var particleSystemMain = _particleSystem.main;
        var newColor = new Color(splashColor.r, splashColor.g, splashColor.b, particleSystemMain.startColor.color.a);
        particleSystemMain.startColor = newColor;
        _particleSystem.Play();
    }
}
