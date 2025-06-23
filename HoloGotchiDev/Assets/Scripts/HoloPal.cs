using System;
using UnityEngine;

// public enum GrowthState
// {
//     Egg,
//     Baby,
//     Child,
//     Adult,
// }

public class HoloPal : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer mesh_renderer;

    public int hunger;
    public int hunger_decay;

    public int total_growth;
    public int stage_growth;
    public int growth_state;

    [SerializeField]
    private int[] growth_stage_thresholds;

    void Update()
    {
        int growth_amount = Math.Min(hunger, hunger_decay);
        
        stage_growth += growth_amount;
        total_growth += growth_amount;
        hunger -= growth_amount;

        if (stage_growth >= growth_stage_thresholds[growth_state] && growth_state < growth_stage_thresholds.Length - 1)
        {
            growth_state++;
            stage_growth = 0;
        }

        mesh_renderer.SetBlendShapeWeight(growth_state, (float)stage_growth / growth_stage_thresholds[growth_state] * 100f);

        // switch (growth_state)
        // {
        //     case GrowthState.Egg:
        //         {
        //             mesh_renderer.SetBlendShapeWeight(0, (float)stage_growth / growth_stage_thresholds[0] * 100f);
        //         }
        //         break;

        //     case GrowthState.Baby:
        //         {
        //             mesh_renderer.SetBlendShapeWeight(1, (float)stage_growth / growth_stage_thresholds[1] * 100f);
        //         }
        //         break;

        //     case GrowthState.Child:
        //         {
        //             mesh_renderer.SetBlendShapeWeight(2, (float)stage_growth / growth_stage_thresholds[2] * 100f);
        //         }
        //         break;

        //     case GrowthState.Adult:
        //         {
        //             mesh_renderer.SetBlendShapeWeight(3, (float)stage_growth / growth_stage_thresholds[3] * 100f);
        //         }
        //         break;

        //     default:
        //         {

        //         }
        //         break;
        // }
    }
}
