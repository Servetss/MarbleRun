using UnityEngine;

public class CoinActivator : MonoBehaviour
{
    [SerializeField] private EventMachine _eventMachine;

    private GameObject[] _coins;

    private void Awake()
    {
        _coins = new GameObject[GetParentCoinCount()];

        int count = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Coin>() == null)
            {
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    _coins[count] = transform.GetChild(i).GetChild(j).gameObject;

                    count++;
                }
            }
            else
            {
                _coins[count] = transform.GetChild(i).gameObject;

                count++;
            }

        }
    }

    void Start()
    {
        _eventMachine?.SubscribeOnMoveToNextLevel(CoinSetActive);
    }

    private int GetParentCoinCount()
    {
        int count = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Coin>() == null)
            {
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    count++;
                }
            }
            else
            {
                count++;
            }
        }

        return count;
    }

    private void CoinSetActive()
    {
        if (gameObject.transform.parent.gameObject.activeSelf)
        {
            for (int i = 0; i < _coins.Length; i++)
            {
                _coins[i].SetActive(true);
            }
        }
    }
}
