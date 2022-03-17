using System.Collections;
using UnityEngine;

public class LiquidDropSpawner : MonoBehaviour
{
    public static LiquidDropSpawner Instance { get; private set; }

    [Header("Drop")]
    [SerializeField] private GameObject _dropPrefab;

    [Header("Params")]
    [Range(0.01f, 5f)] [SerializeField] private float _mass = 1f;
    [Range(0.01f, 5f)] [SerializeField] private float _scale = 0.5f;

    private (Color mainColor, Color outlineColor) _colors;
    private float _delayBetweenSpawn;
    private int _index;

    [Header("Speed & direction")]
    [SerializeField] private Vector2 _initSpeed = Vector2.down;

    public bool Spawning { get; set; } = false;

    private void Start()
    {
        Instance = this;
        StartCoroutine(DropSpawn());
    }

    public void SetParams((Color mainColor, Color outlineColor) colors, float delayBetweenSpawn, int index, bool right)
    {
        _colors = colors;
        _delayBetweenSpawn = delayBetweenSpawn;
        _index = index;
        if (!right) _initSpeed.x *= -1;
    }
    
    private IEnumerator DropSpawn()
    {
        while (true)
        {
            if (Spawning)
            {
                var drop = Instantiate(_dropPrefab, transform.position, Quaternion.identity);
                drop.transform.SetParent(transform.parent);
                drop.GetComponent<LiquidDrop>()
                    .SetParams(_scale, _mass, _initSpeed, _colors, _index);
            }
            yield return new WaitForSeconds(_delayBetweenSpawn);
        }
    }
}
