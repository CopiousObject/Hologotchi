using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatState : IState
{
    GameObject food_target;

    bool eating;

    public EatState(GameObject food_target)
    {
        this.food_target = food_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!eating)
        {
            holopal.Nav_Agent.SetDestination(food_target.transform.position);
        }
    }

    public void OnEnter(HoloPal holopal)
    {
        eating = false;
    }

    public void OnExit(HoloPal holopal)
    {
        eating = false;
        holopal.food = 1f;
        holopal.Nav_Agent.isStopped = false;
        holopal.Spawner.FoodObjects.Remove(food_target);
        Object.Destroy(food_target);
        holopal.state_target = null;
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == food_target && !eating)
        {
            if (holopal.food < 0.8f)
            {
                holopal.state_target = food_target;
                holopal.Nav_Agent.isStopped = true;
                holopal.Nav_Agent.velocity = Vector3.zero;
                holopal.animator.SetTrigger("eating");
                eating = true;
            }
        }
    }
}
