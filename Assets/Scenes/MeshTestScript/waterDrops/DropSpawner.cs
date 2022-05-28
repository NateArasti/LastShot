using System;
using System.Collections;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    public bool IsDropping { get; set; }
    public (Vector2 position, Vector2 direction) DropData { get; set; }

    //private Camera _mainCamera;
    [SerializeField] private WaterDrop _dropPrefab;
    [SerializeField] private int _startDropsCount = 100;
    private UnityObjectPool<WaterDrop> _pool;

    //public Transform _transform;
    //private float _freaquence = 0.05f;
    //private float _cooldown = 0.5f;


    private void Awake()
    {
        _pool = new UnityObjectPool<WaterDrop>(_dropPrefab, transform, _startDropsCount);
    }

    private void Start()
    {
        StartCoroutine(DropSpawn());
    }

    private IEnumerator DropSpawn()
    {
        while (true)
        {
            yield return null;
            if (!IsDropping)
                continue;
            var drop = _pool.GetInstance();
            drop.transform.position = DropData.position;
            //drop.Direction = DropData.direction;

            drop.KillAction.AddListener(_pool.ReleaseInstance);
            
        }
    }
}
