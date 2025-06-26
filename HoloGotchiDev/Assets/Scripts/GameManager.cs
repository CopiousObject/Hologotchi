using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;
    [SerializeField] public int objCount = 10;
    public GameObject foodPrefab;
    public GameObject waterPrefab;
    public List<GameObject> spawnedObjects;
    private GameObject currentObject;


    // Start is called before the first frame update
    void Start()
    {
        currentObject = foodPrefab;
    }

    void Update()
    {
        if (spawnedObjects.Count > objCount)
        {
            Destroy(spawnedObjects[0]);
            spawnedObjects.Remove(spawnedObjects[0]);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            SpawnItem(foodPrefab);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SpawnItem(waterPrefab);
        }
    }

    public void SpawnItem(GameObject current)
    {
        spawnedObjects.Add(Instantiate(current, this.gameObject.transform.position, Quaternion.identity));
    }                                              
}
