using UnityEngine;

public class TrackStats : MonoBehaviour
{
    // Access to each of the sliders for manipulation
    [SerializeField]
    private StatBar thirstBar;
    [SerializeField]
    private StatBar hungerBar;
    [SerializeField]
    private StatBar playBar;
    [SerializeField]
    private StatBar socialBar;
    [SerializeField]
    private StatBar cleanBar;

    // Reciever for IPC Messages
    [SerializeField] private ValholoIPC receiver;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the recieving end
        receiver.OnHandleMessage += HandleMessage;
    }

    private void HandleMessage(IPCMessageId id, string message)
    {
        if (id == IPCMessageId.Stats)
        {
            var stats = message.Split(',');
            hungerBar.slider.value = (float)double.Parse(stats[0]);
            thirstBar.slider.value = (float)double.Parse(stats[1]);
            playBar.slider.value = (float)double.Parse(stats[2]);
            socialBar.slider.value = (float)double.Parse(stats[3]);
            cleanBar.slider.value = (float)double.Parse(stats[4]);
        }
    }
}
