using UnityEngine;

public class SceneObjectsActivator : MonoBehaviour
{
    [SerializeField] private EventMachine _eventMachine;

    private IObjectActivator[] _sceneObjects;

    private void Awake()
    {
        _sceneObjects = new IObjectActivator[GetObjectParentCount()];

        int count = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<IObjectActivator>() == null)
            {
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    _sceneObjects[count] = transform.GetChild(i).GetChild(j).GetComponent<IObjectActivator>();

                    count++;
                }
            }
            else
            {
                _sceneObjects[count] = transform.GetChild(i).GetComponent<IObjectActivator>();

                count++;
            }

        }
    }

    void Start()
    {
        _eventMachine?.SubscribeOnMoveToNextLevel(ObjectsActivate);
    }

    private int GetObjectParentCount()
    {
        int count = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<IObjectActivator>() == null)
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

    private void ObjectsActivate()
    {
        if (gameObject.transform.parent.gameObject.activeSelf)
        {
            for (int i = 0; i < _sceneObjects.Length; i++)
            {
                _sceneObjects[i].Activate();
            }
        }
    }
}
