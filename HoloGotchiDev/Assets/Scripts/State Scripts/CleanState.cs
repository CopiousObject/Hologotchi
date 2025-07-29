using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanState : IState
{
    // Broom object
    GameObject clean_target;

    public CleanState(GameObject clean_target)
    {
        this.clean_target = clean_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        // Exit behavior
        if (!clean_target)
        {
            holopal.ChangeState(null);
            return;
        }
        // Go to broom 
        holopal.Nav_Agent.SetDestination(clean_target.transform.position);
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == clean_target)
        {
            //Play cleaning anim and reverse dirtiness over duration
            holopal.StartCoroutine(Clean(holopal));
            holopal.Spawner.CleanObjects.Remove(clean_target);
            Object.Destroy(other.gameObject);
        }
    }

    // Brings back the opacity of the dirt to 0 over time
    IEnumerator Clean(HoloPal holopal)
    {
        float time = 3;// or length of animation for cleaning
        float elapsed = 0;
        while (elapsed < time)
        {
            Debug.Log("Clean");
            elapsed += Time.deltaTime;
            holopal.clean = Mathf.Lerp(holopal.clean, 1f, elapsed / time);
            yield return null;
        }
    }
}
