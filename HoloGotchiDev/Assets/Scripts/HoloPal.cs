using LookingGlass;
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
    // Sends the messages for IPC
    [SerializeField]
    private InterProcessCommunicator communicator;
    private float lastHungerSent = 0f;
    private float lastThirstSent = 0f;

    [SerializeField]
    private GameManager gameManger;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private ParticleSystem flies;
    [SerializeField]
    private NavMeshAgent nav_agent;
    [SerializeField]
    private NavMeshSurface nav_surface;
    public Vector3 Play_position;

    [SerializeField]
    private SkinnedMeshRenderer mesh_renderer;

    // Hunger tracking fields
    private int food_points;
    [SerializeField]
    private int max_food_points;
    [SerializeField]
    private int food_decay;
    private float hunger => (float)food_points / max_food_points;

    // Thirst tracking fields
    private int water_points;
    [SerializeField]
    private int max_water_points;
    [SerializeField]
    private int water_decay;
    private float thirst => (float)water_points / max_water_points;

    // Thirst tracking fields
    private int play_points;
    [SerializeField]
    private int max_play_points;
    [SerializeField]
    private int play_decay;
    private float playfulness => (float)play_points / max_play_points;

    // Evolution/Growth tracking fields
    [SerializeField]
    private int total_growth;
    [SerializeField]
    private int stage_growth;
    [SerializeField]
    private int growth_state;

    // The thresholds for when the HoloPal will evolve
    [SerializeField]
    private int[] growth_stage_thresholds;

    // Wander related tracking
    [SerializeField]
    private Vector3[] wander_points;
    [SerializeField]
    private float wander_wait_time;

    // private IState[] baby_behaviors = {
    //     new WanderState(3, wander_points)
    // };


    IState current_state;

    // Properties
    public Spawner Spawner => spawner;
    public NavMeshAgent Nav_Agent => nav_agent;
    public int Food_Points { get => food_points; set { food_points = value; } }
    public float Hunger { get => hunger; }
    public int Water_Points { get => water_points; set { water_points = value; } }
    public float Thirst => thirst;
    public float Playfulness => playfulness;
    public GameManager GameManager => gameManger;
    public ParticleSystem Flies => flies;

    /// <summary>
    /// Navigate between the different need states
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IState newState)
    {
        current_state?.OnExit(this);
        current_state = newState;
        current_state?.OnEnter(this);
    }

    /// <summary>
    /// Determines the state changes and the evolution states as the HoloPal grows up
    /// </summary>
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

        // Used for giving values to the IPC receiver
        SendMessages();

        // Figure out how to ease at some point
        if (gameManger.Dirtiness > 80) flies.gameObject.SetActive(true);
        else flies.gameObject.SetActive(false);

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

    private void SendMessages()
    {
        if (Mathf.Abs(lastHungerSent - hunger) >= 1f)
        {
            communicator.SendData("Hunger," + hunger);
            lastHungerSent = hunger;
        }

        //if (Mathf.Abs(lastThirstSent - thirst) >= 0.1f)
        //{
        //    communicator.SendData("Thirst," + thirst);
        //    lastThirstSent = thirst;
        //}
    }
}
