using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;

    // Sends the messages for IPC
    [SerializeField]
    private InterProcessCommunicator communicator;

    // Dirty variables
    [SerializeField]
    private float dirtiness;
    private float dirtTime;
    private float dirtSpeed = 1.5f;

    private float lastSentDirtiness = 0;

    // Properties
    public float Dirtiness { get { return dirtiness; } set { dirtiness = value; } }
    public float DirtSpeed { get { return dirtSpeed; } set { dirtSpeed = value; } }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        dirtiness = 0;
        dirtTime = 0;
    }

    void FixedUpdate()
    {
        dirtTime += Time.deltaTime;
        CalcDirtiness(dirtTime);

        if (Mathf.Abs(lastSentDirtiness - dirtiness) >= 1f)
        {
            communicator.SendData("Dirtiness," + dirtiness);
            lastSentDirtiness = dirtiness;
        }
    }

    // Used for calculating how dirty the holoPal is for 
    private void CalcDirtiness(float dirtTime)
    {
        if (dirtTime >= dirtSpeed)
        {
            if (dirtiness < 100)
            {
                dirtiness++;
            }
            this.dirtTime = 0;
        }
        GetComponent<DecalProjector>().fadeFactor = Mathf.Clamp01(dirtiness / 100f);
    }

}