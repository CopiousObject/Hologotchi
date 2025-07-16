using LookingGlass;
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
    public bool debugMode = false;
    [Range(1, 100)] public float timeMultiplier;
    private float prevTimeMultiplier = 0;

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

    [SerializeField]
    private GameObject eggModel;
    [SerializeField]
    private GameObject holopalMesh;

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
    public GrowthState Growth_State => growth_state;

    private void Start()
    {
        communicator.OnMessageReceived += ReceiveMessage;

        water_points = max_water_points;
        food_points = max_food_points;
        play_points = max_play_points;
        chat_points = max_chat_points;
        chatBubble.alpha = 0f;

        growth_state = GrowthState.Egg;
        if (debugMode)
        {
            growthTime = (Random.Range(1.0f, 6.0f) * 60) / timeMultiplier;
        }
        else
        {
            growthTime = Random.Range(1.0f, 6.0f) * 60;
        }
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
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            if (!debugMode)
            {
                debugMode = true;
            }
            else
            {
                debugMode = false;
            }
        }

        hungerTime += Time.deltaTime;
        thirstTime += Time.deltaTime;
        playTime += Time.deltaTime;
        chatTime += Time.deltaTime;

        // Growth Rework
        switch (growth_state)
        {
            case GrowthState.Egg:
                // Decays
                gameManager.DirtSpeed = 10f;
                // Progress stage
                growthTime -= Time.deltaTime;
                if (growthTime <= 0f)
                {
                    eggModel.SetActive(false);
                    holopalMesh.SetActive(true);

                    growth_state = GrowthState.Baby;
                    if (debugMode)
                    {
                        growthTime = (Random.Range(10f, 20f) * 60f) / timeMultiplier;
                    }
                    else
                    {
                        growthTime = Random.Range(10f, 20f) * 60f;
                    }
                    communicator.SendData("Egg State Exited");
                }
                break;
            case GrowthState.Baby:
                // Decays
                if (debugMode)
                {
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
                    if (chat_points > 0 && chatTime >= 1f)
                    {
                        chat_points -= chat_decay;
                        chatTime -= chatTime;
                    }
                }
                else
                {
                    gameManager.DirtSpeed = 10f;
                    if (water_points > 0 && thirstTime >= 12.5f)
                    {
                        water_points -= water_decay;
                        thirstTime -= thirstTime;
                    }
                    if (food_points > 0 && hungerTime >= 15f)
                    {
                        food_points -= food_decay;
                        hungerTime -= hungerTime;
                    }
                    if (play_points > 0 && playTime >= 15f)
                    {
                        play_points -= play_decay;
                        playTime -= playTime;
                    }
                    if (chat_points > 0 && chatTime >= 10f)
                    {
                        chat_points -= chat_decay;
                        chatTime -= chatTime;
                    }
                }
                // Progress stage
                if (hunger >= 0.85f && thirst >= 0.90f && gameManager.Dirtiness >= 0.60f
                    && chat >= 0.75f && playfulness >= 0.80f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    growth_state = GrowthState.Child;
                    if (debugMode)
                    {
                        growthTime = (Random.Range(20f, 40f) * 60f) / timeMultiplier;
                    }
                    else
                    {
                        growthTime = Random.Range(20f, 40f) * 60f;
                    }

                        mesh_renderer.SetBlendShapeWeight(4, 50);
                }
                break;
            case GrowthState.Child:
                // Decays
                if (debugMode)
                {
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
                }
                else
                {
                    gameManager.DirtSpeed = 10f;
                    if (water_points > 0 && thirstTime >= 10f)
                    {
                        water_points -= water_decay;
                        thirstTime -= thirstTime;
                    }
                    if (food_points > 0 && hungerTime >= 12.5f)
                    {
                        food_points -= food_decay;
                        hungerTime -= hungerTime;
                    }
                    if (play_points > 0 && playTime >= 12.5f)
                    {
                        play_points -= play_decay;
                        playTime -= playTime;
                    }
                    if (chat_points > 0 && chatTime >= 15f)
                    {
                        chat_points -= chat_decay;
                        chatTime -= chatTime;
                    }
                }
                // Progress stage
                if (hunger >= 0.80f && thirst >= 0.85f && gameManager.Dirtiness >= 0.75f
                    && chat >= 0.75f && playfulness >= 0.90f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    growth_state = GrowthState.Adult;
                    if (debugMode)
                    {
                        growthTime = (Random.Range(30f, 60f) * 60f) / timeMultiplier;
                    }
                    else
                    {
                        growthTime = Random.Range(30f, 60f) * 60f;
                    }

                    mesh_renderer.SetBlendShapeWeight(4, 100);
                }
                break;
            case GrowthState.Adult:
                // Decays
                if (debugMode)
                {
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
                }
                else
                {
                    gameManager.DirtSpeed = 15f;
                    if (water_points > 0 && thirstTime >= 12.5f)
                    {
                        water_points -= water_decay;
                        thirstTime -= thirstTime;
                    }
                    if (food_points > 0 && hungerTime >= 15f)
                    {
                        food_points -= food_decay;
                        hungerTime -= hungerTime;
                    }
                    if (play_points > 0 && playTime >= 17.5f)
                    {
                        play_points -= play_decay;
                        playTime -= playTime;
                    }
                    if (chat_points > 0 && chatTime >= 15f)
                    {
                        chat_points -= chat_decay;
                        chatTime -= chatTime;
                    }
                }
                // Progress stage
                if (hunger >= 0.60f && thirst >= 0.70f && gameManager.Dirtiness >= 0.60f
                    && chat >= 0.60f && playfulness >= 0.70f)
                {
                    growthTime -= Time.deltaTime;
                }
                if (growthTime <= 0f)
                {
                    eggModel.SetActive(true);

                    growth_state = GrowthState.Egg;
                    if (debugMode)
                    {
                        growthTime = (Random.Range(1.0f, 6.0f) * 60) / timeMultiplier;
                    }
                    else
                    {
                        growthTime = Random.Range(1.0f, 6.0f) * 60;
                    }

                    holopalMesh.SetActive(false);
                    eggModel.SetActive(true);
                    mesh_renderer.SetBlendShapeWeight(4, 0);

                    communicator.SendData("Egg State Entered");
                }
                break;
        }

        // Clamps to make sure we stay in range
        if (chat_points > 100)
        {
            chat_points = 100;
        }
        if(food_points > 100)
        {
            food_points = 100;
        }
        if(water_points > 100)
        {
            water_points = 100;
        }
        if(play_points > 100)
        {
            play_points = 100;
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

        if(prevTimeMultiplier != timeMultiplier && debugMode)
        {
            prevTimeMultiplier = timeMultiplier;
            growthTime /= timeMultiplier;
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
        if (message == "Stopped Playing")
        {
            play_points += 20;
            ChangeState(null);
        }
    }
}
