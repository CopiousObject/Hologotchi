using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CleanState : IState
{
    // Broom object
    GameObject clean_target;

    bool animating;

    public CleanState(GameObject clean_target)
    {
        this.clean_target = clean_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!animating)
        {
            holopal.Nav_Agent.SetDestination(clean_target.transform.position);
        }
    }

    public void OnEnter(HoloPal holopal)
    {
        animating = false;
    }

    public void OnExit(HoloPal holopal)
    {
        holopal.Spawner.CleanObjects.Remove(clean_target);
        Object.Destroy(clean_target);
        animating = false;
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == clean_target && !animating)
        {
            holopal.animator.SetTrigger("cleaning");
            holopal.held_object = clean_target;
            animating = true;
            //Play cleaning anim and reverse dirtiness over duration
            holopal.StartCoroutine(Clean(holopal));
        }
    }

    // Brings back the opacity of the dirt to 0 over time
    IEnumerator Clean(HoloPal holopal)
    {
        float time = 3;// or length of animation for cleaning
        float elapsed = 0;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            holopal.clean = math.lerp(holopal.clean, 1f, elapsed / time);
            yield return null;
        }
    }
}
