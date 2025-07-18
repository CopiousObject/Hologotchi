using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatState : IState
{
    GameObject chat_target;
    private List<string> babyChats = new List<string> {"Haha!","Aaaaa!","Waaaaa!","wAhHh!"};
    private List<string> childChats = new List<string> { "bababaa...", "da- da- da-", "ma- ma- ma-", "buh buh buh..." };
    private List<string> adultChats = new List<string> { "Papa!", "Mama!", "Hello!", "Love you!"};
    private List<AudioClip> audio;
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
        audio = new List<AudioClip> {holopal.Talk1, holopal.Talk2, holopal.Talk3, holopal.Talk4};
    }

    public void OnExit(HoloPal holopal)
    {
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == chat_target)
        {
            holopal.chat = 1f;
            holopal.Spawner.ChatObjects.Remove(chat_target);
            Object.Destroy(other.gameObject);
        }
    }

    IEnumerator Talk(HoloPal holopal)
    {
        holopal.ChatBubble.alpha = 1f;
        float elapsed = 0f;
        float Total = 3f;
        switch(holopal.current_stage)
        {
            case GrowthStage.Baby:
                holopal.ChatBubble.text = babyChats[Random.Range(0, 3)];
                break;
            case GrowthStage.Child:
                holopal.ChatBubble.text = childChats[Random.Range(0, 3)];
                break;
            case GrowthStage.Adult:
                holopal.ChatBubble.text = adultChats[Random.Range(0, 3)];
                break;
            default:
            break;
        }
        holopal.AudioSource.clip = audio[Random.Range(0, 3)];
        holopal.AudioSource.Play();
        while (elapsed < Total)
        {
            holopal.ChatBubble.rectTransform.rotation = Quaternion.LookRotation(new Vector3(0,0,0),Vector3.up);
            elapsed += Time.deltaTime;
            yield return null;
        }
        holopal.ChatBubble.alpha = 0;
    }
}
