using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatState : IState
{
    GameObject chat_target;

    public ChatState(GameObject chat_target)
    {
        this.chat_target = chat_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!chat_target)
        {
            holopal.StartCoroutine(Talk(holopal));
            holopal.ChangeState(null);
            return;
        }
        holopal.Nav_Agent.SetDestination(chat_target.transform.position);
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == chat_target)
        {
            holopal.Chat_Points = 100;
            holopal.Spawner.ChatObjects.Remove(chat_target);
            Object.Destroy(other.gameObject);
        }
    }

    IEnumerator Talk(HoloPal holopal)
    {
        holopal.ChatBubble.alpha = 1f;
        yield return new WaitForSeconds(3);
        holopal.ChatBubble.alpha = 0;
    }
}
