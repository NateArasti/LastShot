using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    [SerializeField] private GameObject _toggleGameObject;

    public void Toggle() => _toggleGameObject.SetActive(!_toggleGameObject.activeSelf);
}
