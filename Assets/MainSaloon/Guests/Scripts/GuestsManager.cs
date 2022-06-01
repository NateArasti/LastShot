using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuestsManager : MonoBehaviour
{
    [SerializeField] private Dialogue[] _dialogues;
    [Header("Pivots")]
    [SerializeField] private Transform _leftSpawnPoint;
    [SerializeField] private Transform _rightSpawnPoint;
    [SerializeField] private TableTransform[] _tableTransforms;
    [SerializeField] private Transform[] _chairTransforms;
    [Header("Params")]
    [SerializeField] private int _maxGuestAmount = 7;
    [SerializeField] private float _delayBetweenSpawnTry = 3f;

    private readonly HashSet<Guest> _guests = new();
    private HashSet<Transform> _currentlyAvailableChairs;
    private HashSet<TableTransform> _currentlyAvailableTables;
    private HashSet<Dialogue> _availableDialogues = new();

    private void Start()
    {
        _currentlyAvailableChairs = new HashSet<Transform>(_chairTransforms);
        _currentlyAvailableTables = new HashSet<TableTransform>(_tableTransforms);
        _availableDialogues = _dialogues.ToHashSet();
        StartCoroutine(SpawnGuests());
    }

    private IEnumerator SpawnGuests()
    {
        while (true)
        {
            if (_guests.Count < _maxGuestAmount &&
                (_currentlyAvailableChairs.Count > 0 || _currentlyAvailableTables.Count > 0))
            {
                if (_availableDialogues.Count == 0)
                {
                    EndDay();
                    yield break;
                }

                var dialogue = _availableDialogues.GetRandomObject();
                _availableDialogues.Remove(dialogue);
                SpawnGuestsForDialogue(dialogue);
            }
            yield return UnityExtensions.Wait(_delayBetweenSpawnTry);
        }
    }

    private void EndDay()
    {
        Debug.Log("END OF THE DAY");
    }

    private void SpawnGuestsForDialogue(Dialogue dialogue)
    {
        foreach (var character in dialogue.Participants)
        {
            if(character.Prefab == null) continue;
            var guest = Instantiate(character.Prefab, transform);
            guest.SetData(dialogue);
            _guests.Add(guest);
            var right = Random.value > 0.5f;
            guest.transform.position += Vector3.right * (right ? _leftSpawnPoint.position.x : _rightSpawnPoint.position.x);
            guest.OnDestroyEvent.AddListener(ReleaseGuest);
            if (Random.value > 0.5f && _currentlyAvailableChairs.Count > 0) SpawnToChair(guest, right);
            else if (_currentlyAvailableTables.Count > 0) SpawnToTable(guest, right);
            else Destroy(guest.gameObject);
        }
    }

    private void SpawnToChair(Guest spawnedGuest, bool moveRight)
    {
        var spot = _currentlyAvailableChairs.GetRandomObject();
        _currentlyAvailableChairs.Remove(spot);
        spawnedGuest.MoveToSpot(spot, Guest.SpotType.Chair, moveRight);
    }

    private void SpawnToTable(Guest spawnedGuest, bool moveRight)
    {
        var table = _currentlyAvailableTables.GetRandomObject();
        _currentlyAvailableTables.Remove(table);
        spawnedGuest.MoveToSpot(
            moveRight ? table.LeftPivot : table.RightPivot, 
            Guest.SpotType.Table,
            moveRight);
    }

    private void ReleaseGuest(Transform pivot, Guest guest)
    {
        if (_chairTransforms.TryGetObject(
                pivot, 
                (chair, p) => chair.Equals(p),
                out var result1))
        {
            _currentlyAvailableChairs.Add(result1);
        }
        else if (_tableTransforms.TryGetObject(
                pivot, 
                (tableTransform, p) => tableTransform.LeftPivot.Equals(p) || tableTransform.RightPivot.Equals(p),
                out var result2))
        {
            _currentlyAvailableTables.Add(result2);
        }

        _guests.Remove(guest);
    }

    [System.Serializable]
    private struct TableTransform
    {
        [SerializeField] private Transform _leftPivot;
        [SerializeField] private Transform _rightPivot;

        public Transform LeftPivot => _leftPivot;
        public Transform RightPivot => _rightPivot;
    }
}
