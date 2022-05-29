using UnityEngine;
using UnityEngine.UI;

public class UpgradesForceChoose : MonoBehaviour
{
    [SerializeField] private Button _chooseButton;

    private void Start()
    {
        _chooseButton.onClick.Invoke();
    }
}
