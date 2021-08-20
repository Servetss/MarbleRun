using UnityEngine;

public class BoostZoneClicker : IBoost
{
    private bool _isTriggerStay;

    private float _boostIncrease;

    public bool BoostCondition()
    {
        return _isTriggerStay && Input.GetMouseButtonDown(0);
    }

    public void Impulse(Rigidbody rigidbody, int velocity)
    {
        rigidbody.AddForce(rigidbody.velocity.normalized * velocity, ForceMode.Impulse);

        _boostIncrease = rigidbody.velocity.magnitude;

        Debug.Log("Magnitude: " + _boostIncrease);
    }

    public void EnterTrigger()
    {
        _isTriggerStay = true;
    }

    public void ExiteTrigger()
    {
        _isTriggerStay = false;
    }
}
