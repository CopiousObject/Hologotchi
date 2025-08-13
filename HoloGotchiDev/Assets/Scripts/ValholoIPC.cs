using System;
using System.Text;
using LookingGlass;
using UnityEngine;

public enum IPCMessageId
{
    None,
    Stats,
    EggState,
    StartPlay,
    PlayResult,
    ToggleSetting,
    ValueSetting,
    DropItem,
}

[RequireComponent(typeof(InterProcessCommunicator))]
public class ValholoIPC : MonoBehaviour
{
    private InterProcessCommunicator ipc;

    public delegate void HandleMessage(IPCMessageId id, string message);
    public event HandleMessage OnHandleMessage;

    void Awake()
    {
        ipc = GetComponent<InterProcessCommunicator>();

        ipc.OnMessageReceived += ReceiveMessage;
    }

    public void SendStats(double food, double water, double play, double chat, double clean, GrowthStage stage, double growth)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.Stats, food, water, play, chat, clean, stage, growth);

        ipc.SendData(sb.ToString());
    }

    public void SendEggState(bool isEgg)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.EggState, isEgg);

        ipc.SendData(sb.ToString());
    }

    public void SendStartPlay()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.StartPlay);

        ipc.SendData(sb.ToString());
    }

    public void SendPlayResult(int bounce_count)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.PlayResult, bounce_count);

        ipc.SendData(sb.ToString());
    }

    // name could be made an enum for which setting
    public void SendToggleSetting(string name)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.ToggleSetting, name);

        ipc.SendData(sb.ToString());
    }

    // again name could be made an enum for which setting
    public void SendValueSetting(string name, float value)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.ValueSetting, name, value);

        ipc.SendData(sb.ToString());
    }

    public void SendDropItem(string name)
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', IPCMessageId.DropItem, name);

        ipc.SendData(sb.ToString());
    }

    private void ReceiveMessage(string message)
    {
        var message_id_end = message.IndexOf(',');

        // received message with no arguments
        if (message_id_end == -1)
        {
            if (!Enum.TryParse<IPCMessageId>(message, true, out var id))
            {
                Debug.LogError("Unknown IPC message: " + id);
                return;
            }

            Debug.Log("Received message: " + id);
            OnHandleMessage?.Invoke(id, "");
        }
        else
        {
            if (!Enum.TryParse<IPCMessageId>(message.Substring(0, message_id_end), true, out var id))
            {
                Debug.LogError("Unknown IPC message: " + id);
                return;
            }

Debug.Log("Received message: " + id);
            OnHandleMessage?.Invoke(id, message.Substring(message_id_end + 1));
        }
    }
}
