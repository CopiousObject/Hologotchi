using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField]
    private InterProcessCommunicator receiver;

    [SerializeField]
    private GameObject waterButton;
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject chatButton;
    [SerializeField]
    private GameObject foodButton;
    [SerializeField]
    private GameObject cleanButton;

    // Start is called before the first frame update
    void Start()
    {
        receiver.OnMessageReceived += ReceiveMessage;
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ReceiveMessage(string message)
    {
        if (message == "Egg State Exited") Activate();
        if (message == "Egg State Entered") Deactivate();
    }

    public void Activate()
    {
        waterButton.GetComponent<Button>().interactable = true;
        playButton.GetComponent<Button>().interactable = true;
        chatButton.GetComponent<Button>().interactable = true;
        foodButton.GetComponent<Button>().interactable = true;
        cleanButton.GetComponent<Button>().interactable = true;
    }

    public void Deactivate()
    {
        waterButton.GetComponent<Button>().interactable = false;
        playButton.GetComponent<Button>().interactable = false;
        chatButton.GetComponent<Button>().interactable = false;
        foodButton.GetComponent<Button>().interactable = false;
        cleanButton.GetComponent<Button>().interactable = false;
    }
}
