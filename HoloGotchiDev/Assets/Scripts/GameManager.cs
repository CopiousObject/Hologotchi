using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;
    [SerializeField] public int objCount = 10;
    [SerializeField] InterProcessCommunicator receiver;
    public GameObject foodPrefab;
    public GameObject waterPrefab;
    public int max_food_objects;
    public List<GameObject> food_objects;
    public int max_water_objects;
    public List<GameObject> water_objects;
    private GameObject currentObject;

    void Awake()
    {
        food_objects = new List<GameObject>(max_food_objects);
        water_objects = new List<GameObject>(max_water_objects);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentObject = foodPrefab;
        if (receiver != null) receiver.OnMessageReceived += ReceiveMessage;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            SpawnItem(foodPrefab, food_objects);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            SpawnItem(waterPrefab, water_objects);
        }
    }

    public void SpawnItem(GameObject current, List<GameObject> object_list)
    {
        if (object_list.Count == object_list.Capacity)
        {
            var oldest_object = object_list[0];

            oldest_object.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);

            object_list.RemoveAt(0);
            object_list.Add(oldest_object);
            
            return;
        }

        object_list.Add(Instantiate(current, gameObject.transform.position, Quaternion.identity));
    }

    /// <summary>
    /// Processing for IPC
    /// </summary>
    /// <param name="message"></param>
    public void ReceiveMessage(string message)
    {
        if(message == "Drop Item") SpawnItem(foodPrefab, food_objects);
    }
}
