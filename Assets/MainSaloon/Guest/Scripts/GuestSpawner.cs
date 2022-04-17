using UnityEngine;
using UnityEngine.Events;

public class GuestSpawner : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Transform _spawnPivot;
    [SerializeField] private Transform _exitPivot;
    [SerializeField] private UnityEvent<Guest> _onClick;
    [SerializeField] private Transform[] _chairsTransforms;

    private void Start()
    {
        Instantiate(_character.Prefab, _spawnPivot)
            .GetComponent<Guest>().GuestStart(
            _character, 
            _chairsTransforms[3].position.x, 
            //_chairsTransforms[Random.Range(0, _chairsTransforms.Length)].position.x, 
            _onClick);
    }

    public void EndGuestVisit(Guest guest) => guest.EndVisit(_exitPivot.position.x);
}
