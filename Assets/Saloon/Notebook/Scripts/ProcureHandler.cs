using UnityEngine;

public class ProcureHandler : MonoBehaviour
{
    [SerializeField] private RectTransform _spawnPivot;
    [SerializeField] private GameObject _buyPanelPrefab;

    private void Start()
    {
        foreach (var alcohol in DatabaseManager.AlcoholDatabase.GetObjectsCollection())
        {
            Instantiate(_buyPanelPrefab, _spawnPivot)
                .GetComponent<ProcurePanel>().SetProcureData(alcohol);
        }
    }
}
