using UnityEngine;

public class Coin : MonoBehaviour, IObjectActivator
{
    [SerializeField] private GameObject _mesh;

    [SerializeField] private ParticleSystem _particle;

    public void PickUp()
    {
        _mesh.SetActive(false);

        _particle.Play();
    }

    //public void CoinActivate()
    //{
    //    _mesh.SetActive(true);
    //}

    public void Activate()
    {
        _mesh.SetActive(true);
    }
}
