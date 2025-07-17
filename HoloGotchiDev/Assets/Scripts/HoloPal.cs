using System;
using LookingGlass;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public enum GrowthStage
{
    Egg = 0,
    Baby,
    Child,
    Adult,
}

[Serializable]
public struct StageData
{
    [Min(0)]
    public float StageDurationInDays;
    public float MinBlendShape;
    public float MaxBlendShape;

    [Min(0)]
    public float FoodTimesPerDay;
    [Min(0)]
    public float WaterTimesPerDay;
    [Min(0)]
    public float PlayTimesPerDay;
    [Min(0)]
    public float ChatTimesPerDay;
    [Min(0)]
    public float CleanTimesPerDay;
}

public class HoloPal : MonoBehaviour
{
    private const int SECONDS_IN_DAY = 86400;
    private const float DAYS_IN_SECOND = 1f / SECONDS_IN_DAY;

    // Sends the messages for IPC
    [SerializeField]
    private InterProcessCommunicator communicator;

    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private ParticleSystem flies;
    [SerializeField]
    private NavMeshAgent nav_agent;
    [SerializeField]
    private NavMeshSurface nav_surface;
    public Vector3 Play_position;
    public Vector3 startPosition;

    [SerializeField]
    private TextMeshPro chatBubble;

    [SerializeField]
    private SkinnedMeshRenderer mesh_renderer;

    public StageData[] stage_data;
    public GrowthStage current_stage;

    public float growth;
    public float food;
    public float water;
    public float play;
    public float chat;
    public float clean;

    // Wander related tracking
    [SerializeField]
    private Vector3[] wander_points;
    [SerializeField]
    private float wander_wait_time;

    // acting out
    [SerializeField]
    private int act_out_interval_seconds;
    [SerializeField]
    [Range(0, 1)]
    private float act_out_chance;
    [SerializeField]
    private Image overlay_image;
    [SerializeField]
    private Sprite[] broken_glass_images;
    private DateTime next_act_out_time;
    private int act_out_stage;

    [SerializeField]
    private GameObject eggModel;
    [SerializeField]
    private GameObject holopalMesh;

    IState current_state;

    // Properties
    public Spawner Spawner => spawner;
    public NavMeshAgent Nav_Agent => nav_agent;
    public TextMeshPro ChatBubble { get => chatBubble; set { chatBubble = value; } }
    public InterProcessCommunicator Communicator => communicator;

    private void Start()
    {
        communicator.OnMessageReceived += ReceiveMessage;

        food = 1f;
        water = 1f;
        play = 1f;
        chat = 1f;
        clean = 1f;

        chatBubble.alpha = 0f;

        next_act_out_time = DateTime.Now;
    }

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
        growth += Time.deltaTime / (stage_data[(int)current_stage].StageDurationInDays * SECONDS_IN_DAY);

        if (growth >= 1)
        {
            // exceptions for egg state
            if (current_stage == 0)
            {
                eggModel.SetActive(false);
                holopalMesh.SetActive(true);
                communicator.SendData("Egg State Exited");
            }
            if ((int)current_stage == stage_data.Length - 1)
            {
                eggModel.SetActive(true);
                holopalMesh.SetActive(false);
                communicator.SendData("Egg State Entered");

                food = 1f;
                water = 1f;
                play = 1f;
                chat = 1f;
                clean = 1f;
            }

            current_stage = (GrowthStage)(((int)current_stage + 1) % stage_data.Length);
            growth = 0;
        }

        food -= stage_data[(int)current_stage].FoodTimesPerDay * DAYS_IN_SECOND * Time.deltaTime;
        water -= stage_data[(int)current_stage].WaterTimesPerDay * DAYS_IN_SECOND * Time.deltaTime;
        play -= stage_data[(int)current_stage].PlayTimesPerDay * DAYS_IN_SECOND * Time.deltaTime;
        chat -= stage_data[(int)current_stage].ChatTimesPerDay * DAYS_IN_SECOND * Time.deltaTime;
        clean -= stage_data[(int)current_stage].CleanTimesPerDay * DAYS_IN_SECOND * Time.deltaTime;

        food = Mathf.Clamp01(food);
        water = Mathf.Clamp01(water);
        play = Mathf.Clamp01(play);
        chat = Mathf.Clamp01(chat);
        clean = Mathf.Clamp01(clean);

        mesh_renderer.SetBlendShapeWeight(4, Mathf.Lerp(stage_data[(int)current_stage].MinBlendShape, stage_data[(int)current_stage].MaxBlendShape, growth));

        if (chat < 0.15f || play < 0.15f || food < 0.15f || water < 0.15f)
        {
            if (DateTime.Now >= next_act_out_time)
            {
                if (Random.Range(0f, 1f) <= act_out_chance)
                {
                    overlay_image.enabled = true; // UI image with no sprite is a giant white square so disabled by default
                    overlay_image.sprite = broken_glass_images[Mathf.Min(act_out_stage, broken_glass_images.Length - 1)];
                    act_out_stage++;
                }

                next_act_out_time = DateTime.Now + TimeSpan.FromSeconds(act_out_interval_seconds / Time.timeScale);
            }
        }

        // Used for giving values to the IPC receiver
        SendMessages();

        // Figure out how to ease at some point
        if (clean < 0.2f) flies.gameObject.SetActive(true);
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
        communicator.SendData("Hunger," + food);
        communicator.SendData("Thirst," + water);
        communicator.SendData("Play," + play);
        communicator.SendData("Chat," + chat);
        communicator.SendData("Dirtiness," + clean);
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Stopped Playing")
        {
            play = 1f;
            ChangeState(null);
        }
    }
}
