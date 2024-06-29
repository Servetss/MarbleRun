using UnityEngine;

[CreateAssetMenu(fileName = "Skin  Container SO", menuName = "ScriptableObjects/SkinContainer", order = 1)]
public class SkinContainerSO : ScriptableObject
{
    private const int CostStart = 500;

    [SerializeField] private SkinSO[] _skinSO;

    [ContextMenu("Set Prices")]
    public void SetPrices()
    {
        for (int i = 0; i < _skinSO.Length; i++)
        {
            _skinSO[i].SetPrice(CostStart * Mathf.Pow(1.07f, i + 1));
        }
    }
}
