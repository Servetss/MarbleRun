using UnityEngine;

public class Coin : MonoBehaviour, IObjectActivator
{
    [SerializeField] private GameObject _mesh;

    [SerializeField] private GameObject _shadow;

    [SerializeField] private ParticleSystem _particle;

    public void PickUp()
    {
        _shadow.SetActive(false);

        _mesh.SetActive(false);

        _particle.Play();
    }

    //public void CoinActivate()
    //{
    //    _mesh.SetActive(true);
    //}

    public void Activate()
    {
        _shadow.SetActive(true);

        _mesh.SetActive(true);
    }
}
