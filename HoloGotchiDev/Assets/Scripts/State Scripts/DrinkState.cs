using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkState : IState
{
    GameObject water_target;

    bool animating;

    public DrinkState(GameObject water_target)
    {
        this.water_target = water_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!animating)
        {
            holopal.Nav_Agent.SetDestination(water_target.transform.position);
        }
    }

    public void OnEnter(HoloPal holopal)
    {
        animating = false;
    }

    public void OnExit(HoloPal holopal)
    {
        animating = false;
        holopal.food = 1f;
        holopal.Nav_Agent.isStopped = false;
        holopal.Spawner.WaterObjects.Remove(water_target);
        Object.Destroy(water_target);
        holopal.held_object = null;
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == water_target && !animating)
        {
            if (holopal.food < 0.8f)
            {
                holopal.held_object = water_target;
                holopal.Nav_Agent.isStopped = true;
                holopal.Nav_Agent.velocity = Vector3.zero;
                holopal.animator.SetTrigger("drinking");
                animating = true;
            }
        }
    }
}
