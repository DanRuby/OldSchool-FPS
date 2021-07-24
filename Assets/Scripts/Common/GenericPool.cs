using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пул игровых объектов
/// </summary>
/// <typeparam name="T"></typeparam>
public class GenericPool<T> where T : MonoBehaviour
{
    private T objectPrefab = null;
    private List<T> poolOfObjects = new List<T>();

    public GenericPool(int initialPoolSize, T prefab)
    {
        objectPrefab = prefab;
        for (int i = 0; i < initialPoolSize; i++)
        {
            T newObject = Object.Instantiate(prefab);
            newObject.gameObject.SetActive(false);
            poolOfObjects.Add(newObject);
        }
    }

    /// <summary>
    /// Получить не активный объект 
    /// </summary>
    /// <returns></returns>
    public T GetObject()
    {
        T returnObject;
        if (poolOfObjects.Count == 0)
        {
            returnObject = Object.Instantiate(objectPrefab);
        }
        else
        {
            returnObject = poolOfObjects[poolOfObjects.Count - 1];
            poolOfObjects.RemoveAt(poolOfObjects.Count - 1);
        }
        return returnObject;
    }

    /// <summary>
    /// Возвращает объект и отключает его
    /// </summary>
    /// <param name="returnedObject"></param>
    public void ReturnObject(T returnedObject)
    {
        poolOfObjects.Add(returnedObject);
        returnedObject.gameObject.SetActive(false);
    }
}
