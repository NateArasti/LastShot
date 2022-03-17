using UnityEngine;

public class LiquidTouchDetector : MonoBehaviour
{
    private Liquid _liquid;
    private bool _isWorking = true;

    private void Start()
    {
        _liquid = transform.parent.GetComponent<Liquid>();
        _isWorking = _liquid != null;
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (!_isWorking) return;
        if (hit.TryGetComponent<LiquidDrop>(out var liquidDrop))
        {
            if (_liquid.Index != liquidDrop.LiquidIndex)
                _liquid.CreateNewLiquidAbove(liquidDrop.LiquidIndex, liquidDrop.Colors);
            else
                _liquid.Splash(transform.position, liquidDrop.CurrentVelocity, liquidDrop.Mass);
            liquidDrop.Die();
            return;
        }
        if (hit.TryGetComponent<DropItem>(out var itemDrop) && _liquid.Index != 0)
        {
            _liquid.Splash(transform.position, itemDrop.Velocity, itemDrop.Mass);
            itemDrop.TryStartFloating();
            return;
        }
        if (hit.TryGetComponent<GrabDrop>(out var grabDrop))
        {
            if (grabDrop.Destructed) return;
            grabDrop.Destructed = true;
            //_liquid.SpawnStaticLiquid((grabDrop.Data.color, grabDrop.Data.color), grabDrop.Data.mass);
            _liquid.SpawnStaticLiquid((grabDrop.Data.color, grabDrop.Data.color), 1, _liquid.ParentLiquid);
        }
    }
}
