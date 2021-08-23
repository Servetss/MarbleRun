using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Finish _finish;

    private Rigidbody _rigidbody;

    private bool _isJump;

    private float _Yheight;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isJump)
        {
            if (_Yheight >= transform.position.y)
            {
                _isJump = false;

                _rigidbody.isKinematic = true;

                _finish.MarbleStop(GetComponent<Player>());
            }
        }
    }

    public void Impulse(float strenght)
    {
        _rigidbody.isKinematic = false;

        _rigidbody.useGravity = true;

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 10, Color.green, 20);

        _rigidbody.AddForce(((Vector3.up / 3) + transform.forward) * strenght, ForceMode.Impulse);

        Debug.DrawLine(transform.position, transform.position + _rigidbody.velocity * 10, Color.red, 20);

        _Yheight = transform.position.y - 1;

        _isJump = true;
    }
}
