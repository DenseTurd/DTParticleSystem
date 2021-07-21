using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    public virtual T Prefab { get; set; }

    public Queue<T> objects = new Queue<T>();

    public void StartingPool(int count)
    {
        AddObjects(count);
    }

    public T Get()
    {
        if (objects.Count == 0)
            AddObjects(1);
        return objects.Dequeue();
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }

    void AddObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T newObject = GameObject.Instantiate(Prefab);
            newObject.gameObject.SetActive(false);
            objects.Enqueue(newObject);
        }
    }
}
