using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> _objectPool = new Queue<GameObject>();
    public GameObject _objectToPool;
    public int _poolSize;



    private void Start()
    {
        CreatePool();
    }



    public void CleanPool()
    {
        if (_objectPool.Count != 0)
        {
            foreach (var obj in _objectPool)
            {
                obj.SetActive(false);
            }
        }
    }



    private void CreatePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var newObject = Instantiate(_objectToPool);
            newObject.SetActive(false);

            _objectPool.Enqueue(newObject);
        }
    }



    public GameObject GetObjectFromPool(Vector3 pos, Quaternion rot, float force)
    {
        var objectToGet = _objectPool.Dequeue();

        objectToGet.SetActive(true);

        objectToGet.GetComponent<ISpawnable>().SpawnObject(pos, rot, force);

        _objectPool.Enqueue(objectToGet);

        return objectToGet;

    }
}



public interface ISpawnable
{
    public void SpawnObject(Vector3 pos, Quaternion rot, float force);

    public void DestroyObject(GameObject ob);
}