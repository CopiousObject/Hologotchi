using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LeaveState : IState
{
    public void OnEnter(HoloPal holopal)
    {
        holopal.leaving = true;
        holopal.StartCoroutine(Leave(holopal));
    }

    public void OnExit(HoloPal holopal)
    {
        holopal.leaving = false;
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
    }

    public void UpdateState(HoloPal holopal)
    {
    }

    IEnumerator Leave(HoloPal holopal)
    {
        holopal.Nav_Agent.SetDestination(new Vector3(0, -3.39949751f, 2.91000009f));
        holopal.ChatBubble.text = "See ya!";
        holopal.ChatBubble.alpha = 1;
        holopal.transform.LookAt(holopal.mainCamera.transform.position);
        holopal.ChatBubble.rectTransform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), Vector3.up);

        yield return new WaitForSeconds(3f);

        holopal.ChatBubble.alpha = 0;
        holopal.ChatBubble.text = "Sample";
        holopal.Nav_Agent.SetDestination(new Vector3(11.0600004f, -3.39949751f, 2.91000009f));

        yield return new WaitForSeconds(3f);

        holopal.ChangeState(null);
        holopal.GoNextStage();
    }
}
