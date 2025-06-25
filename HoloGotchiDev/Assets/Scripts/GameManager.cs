using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;
    public GameObject foodPrefab;
    public GameObject waterPrefab;
    public List<GameObject> spawnedObjects;



    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (spawnedObjects.Count > 25)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.Remove(spawnedObjects[0]);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            SpawnFood();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SpawnWater();
        }
    }

    public void SpawnFood()
    {
        spawnedObjects.Add(Instantiate(foodPrefab));
    }

    public void SpawnWater()
    {
        spawnedObjects.Add(Instantiate(waterPrefab));
    }

}
