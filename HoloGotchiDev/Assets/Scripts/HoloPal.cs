using LookingGlass;
using System;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public enum GrowthState
{
    Egg,
    Baby,
    Child,
    Adult,
}

public class HoloPal : MonoBehaviour
{
    // Sends the messages for IPC
    [SerializeField]
    private InterProcessCommunicator communicator;

    [SerializeField]
    private GameManager gameManager;
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

    // Play tracking fields
    private int play_points;
    [SerializeField]
    private int max_play_points;
    [SerializeField]
    private int play_decay;
    private float playfulness => (float)play_points / max_play_points;

    private int chat_points;
    [SerializeField]
    private int max_chat_points;
    [SerializeField]
    private int chat_decay;
    private float chat => (float)chat_points / max_chat_points;

    // Evolution/Growth tracking fields
    [SerializeField]
    private int total_growth;
    [SerializeField]
    private int stage_growth;
    [SerializeField]
    private GrowthState growth_state;

    // The thresholds for when the HoloPal will evolve
    [SerializeField]
    private int[] growth_stage_thresholds;

    // Wander related tracking
    [SerializeField]
    private Vector3[] wander_points;
    [SerializeField]
    private float wander_wait_time;

    private float hungerTime;
    private float thirstTime;
    private float playTime;
    private float chatTime;
    private float growthTime;

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
    public int Chat_Points { get => chat_points; set { chat_points = value; } }
    public float Chat => chat;
    public TextMeshPro ChatBubble { get => chatBubble; set { chatBubble = value; } }
    public GameManager GameManager => gameManager;
    public ParticleSystem Flies => flies;
    public InterProcessCommunicator Communicator => communicator;

    private void Start()
    {
        communicator.OnMessageReceived += ReceiveMessage;

        water_points = max_water_points;
        food_points = max_food_points;
        play_points = max_play_points;
        chat_points = max_chat_points;
        chatBubble.alpha = 0f;

        growth_state = GrowthState.Egg;
        growthTime = 60f;
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
    private void FixedUpdate()
    {
        hungerTime += Time.deltaTime;
        thirstTime += Time.deltaTime;
        playTime += Time.deltaTime;
        chatTime += Time.deltaTime;

        // Growth Rework
        switch (growth_state)
        {
            case GrowthState.Egg:
                // Decays
                gameManager.DirtSpeed = 1.25f;
                if (water_points > 0 && thirstTime >= 2.5f)
                {
                    water_points -= water_decay;
                    thirstTime -= thirstTime;
                }
                if (food_points > 0 && hungerTime >= 2.5f)
                {
                    food_points -= food_decay;
                    hungerTime -= hungerTime;
                }
                if (play_points > 0 && playTime >= 2.5f)
                {
                    play_points -= play_decay;
                    playTime -= playTime;
                }
                if (chat_points > 0 && chatTime >= 1f)
                {
                    chat_points -= chat_decay;
                    chatTime -= chatTime;
                }
                // Progress stage
                if (gameManager.Dirtiness >= 0.90f && chat >= 0.85f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    growth_state = GrowthState.Baby;
                    growthTime = 120f;

                    mesh_renderer.SetBlendShapeWeight(4, 33);
                }
                break;
            case GrowthState.Baby:
                // Decays
                gameManager.DirtSpeed = 1f;
                if (water_points > 0 && thirstTime >= 1.25f)
                {
                    water_points -= water_decay;
                    thirstTime -= thirstTime;
                }
                if (food_points > 0 && hungerTime >= 1.5f)
                {
                    food_points -= food_decay;
                    hungerTime -= hungerTime;
                }
                if (play_points > 0 && playTime >= 1.5f)
                {
                    play_points -= play_decay;
                    playTime -= playTime;
                }
                if (chat_points > 0 && chatTime >= 1.75f)
                {
                    chat_points -= chat_decay;
                    chatTime -= chatTime;
                }
                // Progress stage
                if (hunger >= 0.75f && thirst >= 0.80f && gameManager.Dirtiness >= 0.60f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    growth_state = GrowthState.Child;
                    growthTime = 240f;

                    mesh_renderer.SetBlendShapeWeight(4, 66);
                }
                break;
            case GrowthState.Child:
                // Decays
                gameManager.DirtSpeed = 1f;
                if (water_points > 0 && thirstTime >= 1f)
                {
                    water_points -= water_decay;
                    thirstTime -= thirstTime;
                }
                if (food_points > 0 && hungerTime >= 1.25f)
                {
                    food_points -= food_decay;
                    hungerTime -= hungerTime;
                }
                if (play_points > 0 && playTime >= 1.25f)
                {
                    play_points -= play_decay;
                    playTime -= playTime;
                }
                if (chat_points > 0 && chatTime >= 1.5f)
                {
                    chat_points -= chat_decay;
                    chatTime -= chatTime;
                }
                // Progress stage
                if (hunger >= 0.70f && thirst >= 0.70f && gameManager.Dirtiness >= 0.70f
                    && chat >= 0.75f && playfulness >= 0.90f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    growth_state = GrowthState.Adult;
                    growthTime = 480f;

                    mesh_renderer.SetBlendShapeWeight(4, 100);
                }
                break;
            case GrowthState.Adult:
                // Decays
                gameManager.DirtSpeed = 1.5f;
                if (water_points > 0 && thirstTime >= 1.25f)
                {
                    water_points -= water_decay;
                    thirstTime -= thirstTime;
                }
                if (food_points > 0 && hungerTime >= 1.5f)
                {
                    food_points -= food_decay;
                    hungerTime -= hungerTime;
                }
                if (play_points > 0 && playTime >= 1.75f)
                {
                    play_points -= play_decay;
                    playTime -= playTime;
                }
                if (chat_points > 0 && chatTime >= 1.5f)
                {
                    chat_points -= chat_decay;
                    chatTime -= chatTime;
                }
                // Progress stage
                if (hunger >= 0.60f && thirst >= 0.70f && gameManager.Dirtiness >= 0.60f
                    && chat >= 0.60f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    growth_state = GrowthState.Egg;
                    growthTime = 60f;

                    mesh_renderer.SetBlendShapeWeight(4, 0);
                }
                break;
        }

        // Used for giving values to the IPC receiver
        SendMessages();

        // Figure out how to ease at some point
        if (gameManager.Dirtiness > 80) flies.gameObject.SetActive(true);
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
        communicator.SendData("Hunger," + hunger);
        communicator.SendData("Thirst," + thirst);
        communicator.SendData("Play," + playfulness);
        communicator.SendData("Chat," + chat);
    }

    private void ReceiveMessage(string message)
    {
        if(message == "Return HoloPal")
        {
            nav_agent.SetDestination(startPosition);
        }
    }
}
