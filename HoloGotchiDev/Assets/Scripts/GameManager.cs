using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject Holopal;

    public float dirtyness;
    private float dirtTime;
    private float dirtSpeed = 0.5f;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        dirtyness = 0;
        dirtTime = 0;
    }

    void FixedUpdate()
    {
        dirtTime += Time.deltaTime;
        if (dirtTime >= dirtSpeed)
        {
            if (dirtyness < 100)
            {
                dirtyness++;
            }
            dirtTime = 0;
        }

        float normalizedDirtyness = Mathf.Clamp01(dirtyness / 100f);
        GetComponent<DecalProjector>().fadeFactor = normalizedDirtyness;
    }
}