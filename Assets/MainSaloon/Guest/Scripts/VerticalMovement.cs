using System.Collections;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public bool IsMoving { get; private set; }

    [SerializeField] private AnimationCurve _speedCurve;
    [SerializeField] private float _approximateSpeed = 1;
    private Transform _transform;
    private Coroutine _moveCoroutine;
    private float _approximateTime;

    private void Awake()
    {
        _transform = transform;
    }

    public bool TryStartMovement(float newX)
    {
        if (IsMoving) return false;
        _approximateTime = Mathf.Abs(_transform.position.x - newX) / _approximateSpeed;
        _moveCoroutine = StartCoroutine(Move(newX));
        return true;
    }

    public void StopMovement()
    {
        StopCoroutine(_moveCoroutine);
        IsMoving = false;
    }

    private IEnumerator Move(float newX)
    {
        IsMoving = true;
        var time = 0f;
        while (Mathf.Abs(_transform.position.x - newX) > 0.5f)
        {
            var direction = (newX - _transform.position.x) * Vector2.right;
            _transform.Translate(
                _approximateSpeed * Time.deltaTime * _speedCurve.Evaluate(time / _approximateTime) * 
                                 direction.normalized);
            yield return null;
            time += Time.deltaTime;
        }

        var position = _transform.position;
        position.x = newX;
        _transform.position = position;
        IsMoving = false;
    }
}
