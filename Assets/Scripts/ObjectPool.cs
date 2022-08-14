using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> objectPool = new Queue<GameObject>();
    public GameObject ObjectToPool;


    public void CleanPool()
    {
        if (objectPool.Count != 0)
        {
            foreach (var obj in objectPool)
            {
                obj.SetActive(false);
            }
        }
    }



    public void CreatePool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var newObject = Instantiate(ObjectToPool);
            newObject.SetActive(false);

            objectPool.Enqueue(newObject);
        }
    }



    public GameObject GetObjectFromPool(Vector3 pos, Quaternion rot, float force)
    {
        var objectToGet = objectPool.Dequeue();

        objectToGet.SetActive(true);

        objectToGet.GetComponent<ISpawnable>().SpawnObject(pos, rot, force);

        objectPool.Enqueue(objectToGet);

        return objectToGet;

    }
}



public interface ISpawnable
{
    public void SpawnObject(Vector3 pos, Quaternion rot, float force);

    public void DestroyObject(GameObject ob);
}