using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;
    [SerializeField] private InterProcessCommunicator receiver;

    public GameObject foodPrefab;
    public GameObject waterPrefab;
    public int max_food_objects;
    public List<GameObject> food_objects;
    public int max_water_objects;
    public List<GameObject> water_objects;

    public float dirtyness;
    private float dirtTime;
    private float dirtSpeed = 2;

    void Awake()
    {
        food_objects = new List<GameObject>(max_food_objects);
        water_objects = new List<GameObject>(max_water_objects);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Subscribing to IPC message receiver");
        if (receiver != null) receiver.OnMessageReceived += ReceiveMessage;
        dirtyness = 0;
        dirtTime = 0;
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

        dirtTime += Time.deltaTime;
        if (dirtTime >= dirtSpeed)
        {
            if (dirtyness < 100)
            {
                dirtyness++;
            }
            dirtTime = 0;
        }

        float normalizedDirtyness = Mathf.Clamp01(dirtyness / 100f); // Assuming 100 is max dirtyness
        GetComponent<DecalProjector>().fadeFactor = normalizedDirtyness;
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
        Debug.Log("Received IPC message: " + message);
        if (message == "Drop Food Icon") SpawnItem(foodPrefab, food_objects);
        if (message == "Drop Water Icon") SpawnItem(waterPrefab, water_objects);
        if (message == "Drop Chat Icon") SpawnItem(foodPrefab, food_objects);
        if (message == "Drop Ball Icon") SpawnItem(foodPrefab, food_objects);
        if (message == "Drop Kempt Icon") SpawnItem(foodPrefab, food_objects);
    }
}
