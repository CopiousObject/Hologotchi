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
    // [Range(0, 100)]
    // public float MinBlendShape;
    // [Range(0, 100)]
    // public float MaxBlendShape;

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
    public GameObject Model;
}

public class HoloPal : MonoBehaviour
{
    // Stage data
    [Header("Stats and Debug")]
    public StageData[] stage_data;
    public GrowthStage current_stage;

    private bool startGrowth = false;

    public float growth;
    public float food;
    public float water;
    public float play;
    public float chat;
    public float clean;
    [Space]

    public float lowStatThreshold;
    public float criticalStatThreshold;

    [Header("References")]
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

    [Header("Audio")]
    [SerializeField] private AudioClip lowHunger;
    [SerializeField] private AudioClip lowThirst;
    [SerializeField] private AudioClip lowPlay;
    [SerializeField] private AudioClip lowChat;
    [SerializeField] private AudioClip lowClean;
    [SerializeField] private AudioClip talk1;
    [SerializeField] private AudioClip talk2;
    [SerializeField] private AudioClip talk3;
    [SerializeField] private AudioClip talk4;
    [SerializeField]
    private AudioSource audiosource;

    private float notificationVolume;
    private float effectsVolume;

    [Header("Chat Settings")]
    [SerializeField]
    private TextMeshPro chatBubble;
    [SerializeField]
    private Transform chatRotation;

    // Wander related tracking
    [Header("Wander Settings")]
    [SerializeField]
    private Vector3[] wander_points;
    [SerializeField]
    private float wander_wait_time;
    public Vector3 Play_position;
    public Vector3 startPosition;
    [Space]

    // acting out
    [Header("Act Out Settings")]
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

    [Header("Model Settings")]
    public Animator animator;
    public Transform heldObjectTransform;

    [Header("Notification Settings")]
    public bool notifactions;
    public bool notiAudio;
    public bool notiVisual;
    [Space]

    IState current_state;
    [HideInInspector]
    public bool leaving;
    [HideInInspector]
    public GameObject held_object;

    public Camera mainCamera; // for some reason the MainCamera tag was working so temp fix

    // Properties
    public Spawner Spawner => spawner;
    public NavMeshAgent Nav_Agent => nav_agent;
    public TextMeshPro ChatBubble { get => chatBubble; set { chatBubble = value; } }
    public AudioSource AudioSource => audiosource;
    public float NotificationVolume => notificationVolume;
    public float EffectsVolume => effectsVolume;
    public AudioClip Talk1 => talk1;
    public AudioClip Talk2 => talk2;
    public AudioClip Talk3 => talk3;
    public AudioClip Talk4 => talk4;
    public AudioClip LowHunger => lowHunger;
    public AudioClip LowThirst => lowThirst;
    public AudioClip LowPlay => lowPlay;
    public AudioClip LowChat => lowChat;
    public AudioClip LowClean => lowClean;
    public InterProcessCommunicator Communicator => communicator;

    // Used by animations
    public void ExitCurrentState(AnimationEvent animationEvent)
    {
        ChangeState(null);
    }

    public void PickUp(AnimationEvent animationEvent)
    {
        if (held_object)
        {
            held_object.transform.SetParent(heldObjectTransform, false);
            held_object.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            held_object.GetComponent<Rigidbody>().isKinematic = true;
            held_object.GetComponent<Rigidbody>().detectCollisions = false;
        }
    }

    private void Start()
    {
        communicator.OnMessageReceived += ReceiveMessage;

        chatBubble.alpha = 0f;

        notifactions = true;
        notiAudio = true;
        notiVisual = true;

        notificationVolume = 1f;
        effectsVolume = 1f;

        next_act_out_time = DateTime.Now;
        overlay_image.enabled = false; // UI image with no sprite is a giant white square so disabled by default
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

    public void GoNextStage()
    {
        GrowthStage nextStage = (GrowthStage)(((int)current_stage + 1) % stage_data.Length);

        // Setting up for leaving egg state
        if (current_stage == GrowthStage.Egg && nextStage != GrowthStage.Egg)
        {
            communicator.SendData("Egg State Exited");
        }

        // When cycle ends and you enter the egg state again;
        if (nextStage == GrowthStage.Egg && current_stage != GrowthStage.Egg)
        {
            communicator.SendData("Egg State Entered");

            food = 1f;
            water = 1f;
            play = 1f;
            chat = 1f;
            clean = 1f;
        }

        stage_data[(int)current_stage].Model.SetActive(false);
        stage_data[(int)nextStage].Model.SetActive(true);

        current_stage = nextStage;
        growth = 0;
    }

    /// <summary>
    /// Determines the state changes and the evolution states as the HoloPal grows up
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GoNextStage();
        }

        // if (!startGrowth && !debugMode)
        //     {
        //         return;
        //     }

        if (current_stage == GrowthStage.Egg ||
            (current_stage == GrowthStage.Baby && food >= lowStatThreshold && water >= lowStatThreshold && play >= lowStatThreshold && chat >= lowStatThreshold && clean >= lowStatThreshold) ||
            (current_stage == GrowthStage.Child && food >= lowStatThreshold && water >= lowStatThreshold && play >= lowStatThreshold && chat >= lowStatThreshold && clean >= lowStatThreshold) ||
            (current_stage == GrowthStage.Adult && food >= lowStatThreshold && water >= lowStatThreshold && play >= lowStatThreshold && chat >= lowStatThreshold && clean >= lowStatThreshold))
        {
            growth += Time.deltaTime / (stage_data[(int)current_stage].StageDurationInUnits * stage_data[(int)current_stage].SecondsPerUnit);
        }
        
        if (growth >= 1f && !leaving)
        {
            GrowthStage nextStage = (GrowthStage)(((int)current_stage + 1) % stage_data.Length);

            // When cycle ends and you enter the egg state again;
            if (nextStage == GrowthStage.Egg && current_stage != GrowthStage.Egg)
            {
                ChangeState(new LeaveState());
            }
            else
            {
                GoNextStage();
            }
        }

        // Stat weighted for time
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

        // Acting out behavior and trigger
        if (chat < criticalStatThreshold || play < criticalStatThreshold || food < criticalStatThreshold || water < criticalStatThreshold)
        {
            if (DateTime.Now >= next_act_out_time)
            {
                if (Random.Range(0f, 1f) <= act_out_chance)
                {
                    overlay_image.enabled = true;
                    overlay_image.sprite = broken_glass_images[Mathf.Min(act_out_stage, broken_glass_images.Length - 1)];
                    act_out_stage++;
                }

                next_act_out_time = DateTime.Now + TimeSpan.FromSeconds(act_out_interval_seconds / Time.timeScale);
            }
        }

        // Used for giving values to the IPC receiver
        SendMessages();

        // Spawn flies on Holopal when it's dirty... mb find an opacity feature to make this more gradual
        if (clean < 0.2f) flies.gameObject.SetActive(true);
        else flies.gameObject.SetActive(false);

        // Wander -> any state
        if (current_state == null)
        {
            ChangeState(new WanderState(wander_wait_time, wander_points));
        }

        chatRotation.rotation = Quaternion.identity;

        animator.SetFloat("speed", nav_agent.velocity.magnitude); // should this be in a state?
        // Updates current state
        current_state.UpdateState(this);
    }

    // Tells state to trigger data for standing over object
    void OnTriggerEnter(Collider other)
    {
        current_state.OnTriggerEnter(this, other);
    }

    // ^^
    void OnTriggerStay(Collider other)
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
        if (message == "Start Experience") startGrowth = true;
        if (message.Contains("Bounce Count"))
        {
            play += 0.4f * Mathf.Max(int.Parse(message.Substring("Bounce Count ".Length)) / 20f, 1f);
            ChangeState(null);
        }
        if (message == "0") current_stage = GrowthStage.Egg;
        if (message == "1") current_stage = GrowthStage.Baby;
        if (message == "2") current_stage = GrowthStage.Child;
        if (message == "3") current_stage = GrowthStage.Adult;
        if (message.Contains("Time"))
        {
            Debug.Log("Reading");
            string[] splitMessage = message.Split(':');
            float.TryParse(splitMessage[1], out growth);
        }
        if (message == "noti") notifactions = !notifactions;
        if (message == "notiA") notiAudio = !notiAudio;
        if (message == "notiV") notiVisual = !notiVisual;
        if (message.Contains("Notifications"))
        {
            string[] splitMessage = message.Split(',');
            float.TryParse(splitMessage[1], out notificationVolume);
            Debug.Log("Notification Volume" + notificationVolume);
        }
        if (message.Contains("Effects"))
        {
            string[] splitMessage = message.Split(',');
            float.TryParse(splitMessage[1], out effectsVolume);
        }
    }

}
