using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WaypointsPool : MonoBehaviour
{

    public static WaypointsPool Instance;
    private List<GameObject> pool;
    public int maxCap = 4;

    public GameObject Prefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            FillPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FillPool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < maxCap; i++)
        {
            GameObject obj = Instantiate(Prefab);
            obj.SetActive(false);

            pool.Add(obj);
        }
    }

    public static GameObject GetObject()
    {
        GameObject ret = null;

        for (int i = 0; i < Instance.maxCap; i++)
        {
            if (Instance.pool[i].activeInHierarchy == false)
            {
                Instance.pool[i].SetActive(true);
                ret = Instance.pool[i];
                break;
            }
        }

        return ret;
    }

    public static void DeactivateAll()
    {
        for (int i = 0; i < Instance.maxCap; i++)
        {
            if (Instance.pool[i].activeInHierarchy == true)
            {
                Instance.pool[i].SetActive(false);
            }
        }
    }

    public static bool GetIfAnyActivatedPoints() 
    {
        //int activatedPoints = 0;

        for (int i = 0; i < Instance.maxCap; i++) 
        {
            if (Instance.pool[i].activeInHierarchy == true)
            {
                //activatedPoints += 1; 
                return true;
            }
        }
        return false;

        //if (activatedPoints > 0) 
        //{
        //    return true;
        //}

        //return false;
    }
}
