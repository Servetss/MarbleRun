using TMPro;
using UnityEngine;

public class AIPersonalizationView : MonoBehaviour
{
    [SerializeField] private LevelPreparer _levelPreparer;

    [SerializeField] private TextMeshProUGUI _personDataText;
    
    private string GetFlagByIndex(int flagIndex) => $"<sprite name={_brace}flags_{flagIndex}{_brace}>";

    private char _brace => '"';

    private void Start()
    {
        _levelPreparer.OnNextLevel += SetRandomData;

        SetRandomData();
    }

    public void SetRandomData()
    {
        _personDataText.text = PersonalizationBase.Instance.GetRandomName() + GetFlagByIndex(PersonalizationBase.Instance.GetRandomFlagIndex());
    }
}
