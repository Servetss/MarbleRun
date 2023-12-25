using UnityEngine;

public class PersonalizationBase : MonoBehaviour
{
    public static PersonalizationBase Instance;

    private const int FlagIndexCount = 240;

    private string[] _names = new string[] 
    {
        "Liam", "Olivia", "Noah", "Emma", "Oliver", "Charlotte","James", "Amelia", "Elijah","Sophia", "William", "Isabella",
        "Henry", "Ava", "Lucas","Mia", "Benjamin", "Evelyn","Theodore", "Luna"
    };

    private void Awake()
    {
        Instance = this;
    }

    public string GetRandomName()
    {
        return _names[Random.Range(0, _names.Length)];
    }

    public int GetRandomFlagIndex()
    {
        return Random.Range(0, FlagIndexCount);
    }
}
