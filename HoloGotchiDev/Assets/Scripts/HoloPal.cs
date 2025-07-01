using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

// public enum GrowthState
// {
//     Egg,
//     Baby,
//     Child,
//     Adult,
// }

public class HoloPal : MonoBehaviour
{
    public Spawner spawner;
    public NavMeshAgent nav_agent;
    public NavMeshSurface nav_surface;

    [SerializeField]
    private SkinnedMeshRenderer mesh_renderer;

    public int food_points;
    public int max_food_points;
    public int food_decay;

    public int total_growth;
    public int stage_growth;
    public int growth_state;

    [SerializeField]
    private int[] growth_stage_thresholds;

    [SerializeField]
    private Vector3[] wander_points;
    [SerializeField]
    private float wander_wait_time;

    // private IState[] baby_behaviors = {
    //     new WanderState(3, wander_points)
    // };

    public float hunger => food_points / max_food_points;

    IState current_state;

    public void ChangeState(IState newState)
    {
        current_state?.OnExit(this);
        current_state = newState;
        current_state?.OnEnter(this);
    }

    private void Update()
    {
        int growth_amount = Math.Min(food_points, food_decay);

        stage_growth += growth_amount;
        total_growth += growth_amount;
        food_points -= growth_amount;

        if (stage_growth >= growth_stage_thresholds[growth_state] && growth_state < growth_stage_thresholds.Length - 1)
        {
            growth_state++;
            stage_growth = 0;
        }

        mesh_renderer.SetBlendShapeWeight(growth_state, (float)stage_growth / growth_stage_thresholds[growth_state] * 100f);

        if (Input.GetKeyUp(KeyCode.C))
        {
            ChangeState(new CleanState());
            //ChangeState(new WanderState(1,3));
        }

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

        if (current_state == null)
        {
            ChangeState(new WanderState(wander_wait_time, wander_points));
        }

        current_state.UpdateState(this);
    }

    void OnTriggerEnter(Collider other)
    {
        current_state.OnTriggerEnter(this, other);
    }
}
