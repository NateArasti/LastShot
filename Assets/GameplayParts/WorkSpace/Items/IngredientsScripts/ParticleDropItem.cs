using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDropItem : MonoBehaviour
{
    [SerializeField] private Material _particleMaterial;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetItem(Color particleColor)
    {
        _particleMaterial.color = particleColor;
        _particleSystem.Play();
    }
}
