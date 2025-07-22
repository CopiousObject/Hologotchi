using LookingGlass;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public Camera currentCam;
    public InterProcessCommunicator Communicator;
    public UIAnimation uiAnimation;
    public Transform HoloPalTarget;

    public GameObject PlayAssets;
    public Collider2D TopBound;
    public Collider2D LeftBound;
    public Collider2D RightBound;
    public Collider2D BottomBound;
    public GameObject BallObject;
    public GameObject PlayerPaddle;
    public GameObject HoloPaddle;
    public GameObject StartButton;

    private Vector3 Paddle1Pos; 
    private Vector3 Paddle2Pos; 
    private Vector3 BallStartPos = new Vector3(-0.5f, 2.68000007f, 90f);
    private bool Playing;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        Playing = false;
        Communicator.OnMessageReceived += ReceiveMessage;
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Picked up ball")
        {
            uiAnimation.NavigateToBall();

            // make elements visible
            StartButton.SetActive(true);
            PlayAssets.SetActive(true);
        }
    }

    public void StartGame()
    {
        StartButton.SetActive(false);
        time = 0;
        
        // Reset positions
        HoloPaddle.transform.localPosition = new Vector3(-0.5f, 42f, 90f);
        PlayerPaddle.transform.localPosition  = new Vector3(-0.5f, -44f, 90f);
        BallObject.transform.localPosition = BallStartPos;

        // Ball start
        Vector2 randomDir = new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f,1.0f));
        BallObject.GetComponent<Rigidbody2D>().AddForce(randomDir * 250);

        Playing = true;
    }

    void Update()
    {
        if (Playing)
        {
            // Mouse tracking (very complicated for some reason)
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Mathf.Abs(currentCam.transform.position.z - PlayerPaddle.transform.localPosition.z);
            Vector3 mouseWorldPos = currentCam.ScreenToWorldPoint(mouseScreenPos);
            PlayerPaddle.transform.localPosition = new Vector3(mouseWorldPos.x + 100, PlayerPaddle.transform.localPosition.y, PlayerPaddle.transform.localPosition.z);

            // Holopal paddle "AI" 
            

            // Game logic
            

            time += Time.deltaTime;
            if (time > 50.0f)
            {
                Communicator.SendData("Stopped Playing");
                PlayAssets.SetActive(false);
                Playing = false;
                uiAnimation.NavigateToPlay();
            }
        }
    }
}
