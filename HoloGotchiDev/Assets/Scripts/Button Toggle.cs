using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField]
    private ValholoIPC receiver;

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
        receiver.OnHandleMessage += HandleMessage;
        Deactivate();
    }

    void HandleMessage(IPCMessageId id, string message)
    {
        if (id == IPCMessageId.EggState)
        {
            var isEgg = bool.Parse(message);

            if (isEgg)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }
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
