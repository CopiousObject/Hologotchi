using LookingGlass;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public InterProcessCommunicator Communicator;
    public UIAnimation uiAnimation;
    public float TChange;
    public Transform HoloPalTarget;

    public GameObject PlayAssets;
    public GameObject BallObject;
    public GameObject HoloPalObject;
    public GameObject StartButton;

    private Vector3 BallStartPos;
    private Vector3 HoloPalStartPos;

    private Vector3 LastBallPos;
    private float T;

    private bool Playing;

    // Start is called before the first frame update
    void Start()
    {
        Communicator.OnMessageReceived += ReceiveMessage;

        BallStartPos = BallObject.transform.position;
        HoloPalStartPos = HoloPalObject.transform.position;
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Picked up ball")
        {
            uiAnimation.NavigateToBall();

            BallObject.transform.position = BallStartPos;
            HoloPalObject.transform.position = HoloPalStartPos;

            PlayAssets.SetActive(true);
            StartButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        StartButton.SetActive(false);

        LastBallPos = BallObject.transform.position;
        T = 0;
        TChange = TChange * TChange; // make positive
        TChange = Mathf.Sqrt(TChange);

        Playing = true;
    }

    void Update()
    {
        if (Playing)
        {
            if (T > 0.9f)
            {
                TChange = -TChange;
            }

            BallObject.transform.position = Vector3.Slerp(LastBallPos, HoloPalTarget.transform.position, T);

            T += TChange * Time.deltaTime;

            if (T <= 0f)
            {
                Communicator.SendData("Stopped Playing");
                Playing = false;
                PlayAssets.SetActive(false);
                uiAnimation.NavigateToPlay();
            }
        }
    }
}
