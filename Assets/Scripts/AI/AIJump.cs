using UnityEngine;

public class AIJump : MonoBehaviour
{
    private EventMachine _eventMachine;

    private RoadMover _roadMover;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _roadMover = GetComponent<RoadMover>();

        _eventMachine = GetComponent<EventMachine>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _eventMachine?.SubscribeOnRoadEnd(JumpStart);
    }

    public void JumpStart()
    {
        _rigidbody.isKinematic = false;

        _rigidbody.useGravity = true;

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 10, Color.green, 20);

        _rigidbody.AddForce(((Vector3.up / 3) + transform.forward) * 40, ForceMode.Impulse);

        Debug.DrawLine(transform.position, transform.position + _rigidbody.velocity * 10, Color.red, 20);
    }
}
