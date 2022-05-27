using System.Collections.Generic;
using UnityEngine;

public class ScriptableManagerReference : MonoBehaviour
{
    [SerializeField] private List<ScriptableSingleton> _referenceScriptableSingletons;

    private void Awake()
    {
        foreach (var scriptableSingleton in _referenceScriptableSingletons)
        {
            scriptableSingleton.SetAsInstance();
        }
    }
}
