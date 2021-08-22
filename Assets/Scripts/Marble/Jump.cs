using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Impulse(float strenght)
    {
        _rigidbody.isKinematic = false;

        _rigidbody.useGravity = true;

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 10, Color.green, 20);

        _rigidbody.AddForce(((Vector3.up / 3) + transform.forward) * strenght, ForceMode.Impulse);

        Debug.DrawLine(transform.position, transform.position + _rigidbody.velocity * 10, Color.red, 20);
    }
}
