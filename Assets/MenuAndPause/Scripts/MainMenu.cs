using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Guest")]
    [SerializeField] private Guest _guest;
    [SerializeField] private Transform _sitSpot;

    private void Start()
    {
        _guest.MoveToSpot(_sitSpot, Guest.SpotType.Chair, false, false);
    }
}
