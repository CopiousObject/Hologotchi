using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bend_test : MonoBehaviour
{
    [SerializeField]
    private float blend_speed;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private float blend;

    // Start is called before the first frame update
    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
        {
        skinnedMeshRenderer.SetBlendShapeWeight(i, blend);
        }

        blend += Time.deltaTime * blend_speed;
        if (blend >= 100)
        {
            blend = 0;
        }
    }
}
