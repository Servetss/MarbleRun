using UnityEngine;

public class TestBoost : IBoost
{ 
    public void Init(MarbleImpulse marbleImpulse)
    {

    }

    public bool BoostCondition()
    {
        return Input.GetMouseButton(0);
    }

    public void Impulse(Rigidbody rigidbody, int velocity)
    {
        rigidbody.AddForce(rigidbody.velocity.normalized * velocity);
    }

    public void EnterTrigger()
    {

    }

    public void ExiteTrigger()
    {

    }
}
