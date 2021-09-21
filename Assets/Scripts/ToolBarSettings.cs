using UnityEngine;

public class ToolBarSettings : MonoBehaviour
{
    [ContextMenu("Reset save")]
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
