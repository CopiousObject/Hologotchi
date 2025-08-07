using LookingGlass;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayManager : MonoBehaviour
{
    public Camera currentCam;
    public InterProcessCommunicator Communicator;
    public UIAnimation uiAnimation;

    public GameObject PlayAssets;
    public Collider2D TopBound;
    public Collider2D LeftBound;
    public Collider2D RightBound;
    public Collider2D BottomBound;
    public GameObject BallObject;
    public GameObject PlayerPaddle;
    public GameObject HoloPaddle;
    public GameObject StartButton;

    private string message;
    private Vector3 Paddle1Pos;
    private Vector3 Paddle2Pos;
    private Vector3 BallStartPos = new Vector3(-0.5f, 2.68000007f, 90f);
    private bool Playing;
    private bool Fin;

    void Start()
    {
        Playing = false;
        Communicator.OnMessageReceived += ReceiveMessage;
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Picked up ball")
        {
            uiAnimation.NavigateToPlay();

            // make elements visible
            StartButton.SetActive(true);
            StartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
            StartButton.GetComponent<Selectable>().interactable = true;
            PlayAssets.SetActive(true);
        }
    }

    public void StartGame()
    {
        StartButton.SetActive(false);

        // Reset positions
        HoloPaddle.transform.localPosition = new Vector3(-0.5f, 38f, 90f);
        PlayerPaddle.transform.localPosition = new Vector3(-0.5f, -38f, 90f);
        BallObject.transform.localPosition = BallStartPos;

        // Ball start
        Vector2 randomDir = new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f));
        BallObject.GetComponent<Rigidbody2D>().AddForce(randomDir * 250);

        Playing = true;
        Fin = false;
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
            float holoSpeed = 10f; // AI paddle speed
            Vector3 holoPos = HoloPaddle.transform.localPosition;
            float targetX = BallObject.transform.localPosition.x;
            float newX = Mathf.Lerp(holoPos.x, targetX, Time.deltaTime * holoSpeed);
            HoloPaddle.transform.localPosition = new Vector3(newX, holoPos.y, holoPos.z);

            // Ball hit scoring bounds
            if (!Fin && (TopBound.GetComponent<BoxCollider2D>().IsTouching(BallObject.GetComponent<Collider2D>()))
                || BottomBound.GetComponent<BoxCollider2D>().IsTouching(BallObject.GetComponent<Collider2D>()))
            {
                BallObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                StartCoroutine(End());
            }
        }
    }

    IEnumerator End()
    {
        // Resets all components and shows player that the game is ending
        StartButton.SetActive(true);
        StartButton.GetComponent<Selectable>().interactable = false;
        StartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Done!";
        float time = 0;
        while (time < 3f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Communicator.SendData("Bounce Count " + BallObject.GetComponent<BallSpeed>().bounceCount);
        PlayAssets.SetActive(false);
        Playing = false;
        uiAnimation.NavigateToMain();
    }
}
