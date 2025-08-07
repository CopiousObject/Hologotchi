using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : IState
{
    private float duration;
    private Vector3[] wander_points;

    private float timer;

    private bool notifications;
    private bool notiAudio;
    private bool notiVideo;

    public WanderState(float duration, Vector3[] wander_points)
    {
        this.duration = duration;
        this.wander_points = wander_points;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (holopal.Nav_Agent.hasPath && holopal.Nav_Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            return;
        }

        // Manages state triggers from wander
        if (holopal.food < 0.8f && holopal.Spawner.FoodObjects.Count > 0)
        {
            holopal.ChangeState(new EatState(holopal.Spawner.FoodObjects[0]));
            return;
        }


        if (holopal.water < 0.8f && holopal.Spawner.WaterObjects.Count > 0)
        {
            holopal.ChangeState(new DrinkState(holopal.Spawner.WaterObjects[0]));
            return;
        }

        if (holopal.play < 0.8f && holopal.Spawner.PlayObjects.Count > 0)
        {
            holopal.ChangeState(new PlayState(holopal.Spawner.PlayObjects[0]));
            return;
        }

        if (holopal.chat < 0.8f && holopal.Spawner.ChatObjects.Count > 0)
        {
            holopal.ChangeState(new ChatState(holopal.Spawner.ChatObjects[0]));
            return;
        }

        if (holopal.clean < 0.8f && holopal.Spawner.CleanObjects.Count > 0)
        {
            holopal.ChangeState(new CleanState(holopal.Spawner.CleanObjects[0]));
            return;
        }

        if (timer <= 0f)
        {
            // Every 3 seconds has a 1/3 chance to move to a new spot
            if (Random.Range(0, 3) == 0)
            {
                holopal.Nav_Agent.SetDestination(wander_points[Random.Range(0, wander_points.Length)]);
            }

            // Crying
            if ((holopal.food < 0.4f && holopal.Spawner.FoodObjects.Count == 0 && holopal.notifactions)
                        || (holopal.water < 0.4f && holopal.Spawner.WaterObjects.Count == 0)
                        || (holopal.play < 0.4f && holopal.Spawner.PlayObjects.Count == 0)
                        || (holopal.chat < 0.4f && holopal.Spawner.ChatObjects.Count == 0)
                        || (holopal.clean < 0.4f && holopal.Spawner.CleanObjects.Count == 0))
            {
                holopal.StartCoroutine(LowStat(holopal));
            }

            timer = duration;
        }

        timer -= Time.deltaTime;
    }

    public void OnEnter(HoloPal holopal)
    {
        holopal.Nav_Agent.speed = 2;
        timer = duration;
    }

    public void OnExit(HoloPal holopal)
    {
        holopal.StartCoroutine(Leave(holopal));
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {

    }

    // Annoyed whining to get player attention from neglect
    IEnumerator LowStat(HoloPal holopal)
    {

        if (holopal.notiVisual) holopal.ChatBubble.alpha = 1f;

        // Determine which audio sound to play
        if (holopal.food < 0.4f && holopal.Spawner.FoodObjects.Count == 0) holopal.AudioSource.clip = holopal.LowHunger;
        else if (holopal.water < 0.4f && holopal.Spawner.WaterObjects.Count == 0) holopal.AudioSource.clip = holopal.LowThirst;
        else if (holopal.play < 0.4f && holopal.Spawner.PlayObjects.Count == 0) holopal.AudioSource.clip = holopal.LowPlay;
        else if (holopal.chat < 0.4f && holopal.Spawner.ChatObjects.Count == 0) holopal.AudioSource.clip = holopal.LowChat;
        else if (holopal.clean < 0.4f && holopal.Spawner.CleanObjects.Count == 0) holopal.AudioSource.clip = holopal.LowClean;

        float elapsed = 0f;
        float Total = 3f;
        while (elapsed < Total)
        {
            // Trigger an animation for this ig
            holopal.ChatBubble.rectTransform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), Vector3.up);
            if (holopal.notiAudio)
            {
                holopal.ChatBubble.text = "I can't grow";
                holopal.AudioSource.volume = holopal.NotificationVolume;
                holopal.AudioSource.Play();
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        holopal.ChatBubble.alpha = 0;
        holopal.ChatBubble.text = "Sample";
    }

    IEnumerator Leave(HoloPal holopal)
    {
        holopal.Nav_Agent.SetDestination(new Vector3(0, -3.39949751f, 2.91000009f));
        float elapsed = 0f;
        float Total = 3f;
        holopal.ChatBubble.text = "See ya!";
        holopal.ChatBubble.alpha = 1;
        holopal.transform.LookAt(Camera.main.transform.position);
        while (elapsed < Total)
        {
            holopal.ChatBubble.rectTransform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), Vector3.up);
            elapsed += Time.deltaTime;
            yield return null;
        }
        holopal.ChatBubble.alpha = 0;
        holopal.ChatBubble.text = "Sample";
        holopal.Nav_Agent.SetDestination(new Vector3(11.0600004f, -3.39949751f, 2.91000009f));
        elapsed = 0f;
        while (elapsed < Total)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
