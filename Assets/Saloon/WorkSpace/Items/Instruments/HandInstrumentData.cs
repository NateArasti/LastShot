using UnityEngine;

public class HandInstrumentData : MonoBehaviour
{
    [SerializeField] private Vector2 _offSet;
    [SerializeField] private float _xClamp;

    public (Vector2 offSet, float xClamp) SpoonData => (_offSet, _xClamp);
}
