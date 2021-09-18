using UnityEngine;

public class AIMarbleTrigger : MonoBehaviour
{
    [SerializeField] private AI _marbleAI;

    private void OnTriggerEnter(Collider other)
    {
        _marbleAI?.OnMarbleSphereTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _marbleAI?.OnMarbleSphereTriggerEnter(other);
    }
}
