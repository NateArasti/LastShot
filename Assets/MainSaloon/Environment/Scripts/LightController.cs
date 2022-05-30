using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Vector2 _globalLightMinMax;
    [SerializeField] private Light2D[] _lampLights;
    [SerializeField] private float _lampsOnTriggerValue = 0.7f;
    private bool _lampsOn;

    private void Awake()
    {
        ToggleLamps(false);
    }

    private void Update()
    {
        _globalLight.intensity = Mathf.Lerp(_globalLightMinMax.y, _globalLightMinMax.x, GameTimeController.CurrentTime);
        if (!_lampsOn && _globalLight.intensity <= _lampsOnTriggerValue)
        {
            ToggleLamps(true);
            _lampsOn = true;
        }
    }

    private void ToggleLamps(bool active)
    {
        _lampLights.ForEachAction(lamp => lamp.gameObject.SetActive(active));
    }

    [ContextMenu("Toggle Lamps")]
    public void Toggle()
    {
        ToggleLamps(!_lampLights[0].gameObject.activeSelf);
    }
}
