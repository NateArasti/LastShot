using UnityEngine;

public class CocktailSpoon : MonoBehaviour
{
    public void ChooseSpoonLayerEvent()
    {
        OrderCreationEvents.Instance.SwitchToSpoonLayerEvent();
        Destroy(gameObject);
    }

    public void ChooseSpoonMixEvent()
    {
        OrderCreationEvents.Instance.SwitchToSpoonMixEvent();
        Destroy(gameObject);
    }
}
