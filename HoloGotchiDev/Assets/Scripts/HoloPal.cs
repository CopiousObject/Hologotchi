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
    public int SecondsPerUnit;
    [Min(0)]
    public float StageDurationInUnits;
    [Range(0, 100)]
    public float MinBlendShape;
    [Range(0, 100)]
    public float MaxBlendShape;

    [Min(0)]
    public float FoodTimesPerUnit;
    [Min(0)]
    public float WaterTimesPerUnit;
    [Min(0)]
    public float PlayTimesPerUnit;
    [Min(0)]
    public float ChatTimesPerUnit;
    [Min(0)]
    public float CleanTimesPerUnit;
}

public class HoloPal : MonoBehaviour
{
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

    [Header("Audio Files")]
    [SerializeField] private AudioClip annoyed;
    [SerializeField] private AudioClip talk1;
    [SerializeField] private AudioClip talk2;
    [SerializeField] private AudioClip talk3;
    [SerializeField] private AudioClip talk4;

    [SerializeField]
    private TextMeshPro chatBubble;
    [SerializeField]
    private Transform chatRotation;

    [SerializeField]
    private SkinnedMeshRenderer mesh_renderer;

    [SerializeField]
    private AudioSource audiosource;

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
    public AudioSource AudioSource => audiosource;
    public AudioClip Talk1 => talk1;
    public AudioClip Talk2 => talk2;
    public AudioClip Talk3 => talk3;
    public AudioClip Talk4 => talk4;
    public AudioClip Annoyed => annoyed;
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
        eggModel.isStatic = false;
        holopalMesh.isStatic = false;

        if (current_stage == GrowthStage.Egg ||
            (current_stage == GrowthStage.Baby && food >= 0.85 && water >= 0.85 && play >= 0.80 && chat >= 0.50 && clean >= 0.85) ||
            (current_stage == GrowthStage.Child && food >= 0.80 && water >= 0.80 && play >= 0.85 && chat >= 0.68 && clean >= 0.75) ||
            (current_stage == GrowthStage.Adult && food >= 0.75 && water >= 0.75 && play >= 0.75 && chat >= 0.85 && clean >= 0.65))
        {
            growth += Time.deltaTime / (stage_data[(int)current_stage].StageDurationInUnits * stage_data[(int)current_stage].SecondsPerUnit);
        }

        if (growth >= 1)
        {
            GrowthStage previousStage = current_stage;
            GrowthStage nextStage = (GrowthStage)(((int)current_stage + 1) % stage_data.Length);

            // Setting up for leaving egg state
            if (previousStage == GrowthStage.Egg && nextStage != GrowthStage.Egg)
            {
                eggModel.SetActive(false);
                holopalMesh.SetActive(true);
                communicator.SendData("Egg State Exited");
            }
            // When cycle ends and you enter the egg state again;
            if (nextStage == GrowthStage.Egg && previousStage != GrowthStage.Egg)
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

            current_stage = nextStage;
            growth = 0;
        }

        var stage_units_per_second = 1f / stage_data[(int)current_stage].SecondsPerUnit;
        food -= stage_data[(int)current_stage].FoodTimesPerUnit * stage_units_per_second * Time.deltaTime;
        water -= stage_data[(int)current_stage].WaterTimesPerUnit * stage_units_per_second * Time.deltaTime;
        play -= stage_data[(int)current_stage].PlayTimesPerUnit * stage_units_per_second * Time.deltaTime;
        chat -= stage_data[(int)current_stage].ChatTimesPerUnit * stage_units_per_second * Time.deltaTime;
        clean -= stage_data[(int)current_stage].CleanTimesPerUnit * stage_units_per_second * Time.deltaTime;

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

        chatRotation.rotation = Quaternion.identity;

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
        communicator.SendData("{0}", (int)current_stage);
        communicator.SendData("Time:" + growth);
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Stopped Playing")
        {
            play = 1f;
            ChangeState(null);
        }
        if (message == "0") current_stage = GrowthStage.Egg;
        if (message == "1") current_stage = GrowthStage.Baby;
        if (message == "2") current_stage = GrowthStage.Child;
        if (message == "3") current_stage = GrowthStage.Adult;
        if (message.Contains("Time"))
        {
            string[] splitMessage = message.Split(':');
            float.TryParse(splitMessage[1], out growth);
        }
    }
}
