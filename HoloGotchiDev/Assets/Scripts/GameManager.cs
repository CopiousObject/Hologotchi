using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;

    // Dirty variables
    public float dirtiness;
    private float dirtTime;
    private float dirtSpeed = 2f;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        dirtiness = 0;
        dirtTime = 0;
    }

    void Update()
    {
        dirtTime += Time.deltaTime;
        Dirtiness(dirtTime);
    }

    // Used for calculating how dirty the holoPal is for 
    private void Dirtiness(float dirtTime)
    {
        if (dirtTime >= dirtSpeed)
        {
            if (dirtiness < 100)
            {
                dirtiness++;
}
            
            }
        dirtTime = 0;

        GetComponent<DecalProjector>().fadeFactor = Mathf.Clamp01(dirtiness / 100f);
    }

}