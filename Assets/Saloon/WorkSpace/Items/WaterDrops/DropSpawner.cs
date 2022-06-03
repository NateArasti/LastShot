﻿using System.Collections;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private Material _dropMaterial;
    [SerializeField] private WaterDrop _dropPrefab;
    [SerializeField] private int _startDropsCount = 100;
    private UnityObjectPool<WaterDrop> _pool;
    public bool IsDropping { get; set; }
    public (Vector2 position, Vector2 direction) DropData { get; set; }

    public Color DropColor
    {
        get => _dropPrefab.DropColor;
        set => _dropPrefab.DropColor = value;
    }

    public float DropDelay { get; set; }

    private void Awake()
    {
        _pool = new UnityObjectPool<WaterDrop>(_dropPrefab, transform, _startDropsCount);
        DropDelay = 0;
    }

    private void Start()
    {
        StartCoroutine(DropSpawn());
    }
    private IEnumerator DropSpawn()
    {
        while (true)
        {
            yield return UnityExtensions.Wait(DropDelay);
            if (!IsDropping)
                continue;
            _dropMaterial.color = DropColor;
            var drop = _pool.GetInstance();
            drop.transform.position = DropData.position;
            drop.Direction = DropData.direction;
            drop.DropColor = DropColor;
            drop.KillAction.AddListener(_pool.ReleaseInstance);
        }
    }
}