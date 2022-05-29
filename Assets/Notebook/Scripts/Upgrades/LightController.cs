using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public enum DayState
    {
        Day,
        Evening,
        Night
    }

    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Light2D[] _lights;

    public void SetTime(int state)
    {
        var dayState = (DayState) state;
        switch (dayState)
        {
            case DayState.Day:
                _lights.ForEachAction(l => l.enabled = false);
                _globalLight.intensity = 0.9f;
                break;
            case DayState.Evening:
                _lights.ForEachAction(l => l.enabled = true);
                _globalLight.intensity = 0.5f;
                break;
            case DayState.Night:
                _lights.ForEachAction(l => l.enabled = true);
                _globalLight.intensity = 0.1f;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dayState), dayState, null);
        }
    }
}
