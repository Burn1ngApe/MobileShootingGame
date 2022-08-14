using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Queue<GameObject> MyObjectPool = new Queue<GameObject>();
    public GameObject ObjectToPool;


    public void CleanPool()
    {
        if (MyObjectPool.Count != 0)
        {
            foreach (var obj in MyObjectPool)
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

            MyObjectPool.Enqueue(newObject);
        }
    }



    public GameObject GetObjectFromPool(Vector3 pos, Quaternion rot, float force)
    {
        var objectToGet = MyObjectPool.Dequeue();

        objectToGet.SetActive(true);

        objectToGet.GetComponent<ISpawnable>().SpawnObject(pos, rot, force);

        MyObjectPool.Enqueue(objectToGet);

        return objectToGet;

    }
}



public interface ISpawnable
{
    public void SpawnObject(Vector3 pos, Quaternion rot, float force);

    public void DestroyObject(GameObject ob);
}