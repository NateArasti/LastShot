using System.Collections;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
public class LightShow : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D[] _lamps;
    private UnityEngine.Rendering.Universal.Light2D _globalLight;
    private bool _lampsTurnedOn;

    private void Awake()
    {
        _globalLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        SetLight(0);
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        while(_globalLight.intensity > 0.5f)
        {
            _globalLight.intensity -= _speed * Time.deltaTime;
            if (_globalLight.intensity <= 1f)
            {
                foreach (var lamp in _lamps)
                {
                    lamp.gameObject.SetActive(true);
                }
            }
            yield return null;
        }
    }

    public void SetLight(int index)
    {
        if(index == 0)
        {
            _globalLight.intensity = 1.5f;
            foreach(var lamp in _lamps)
            {
                lamp.gameObject.SetActive(false);
            }
        }
        else
        {
            if(index == 1)
                _globalLight.intensity = 1f;
            else
                _globalLight.intensity = 0.5f;
            foreach (var lamp in _lamps)
            {
                lamp.gameObject.SetActive(true);
            }
        }
    }
}
