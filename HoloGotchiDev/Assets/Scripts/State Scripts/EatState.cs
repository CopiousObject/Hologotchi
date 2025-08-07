using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatState : IState
{
    GameObject food_target;

    bool animating;

    public EatState(GameObject food_target)
    {
        this.food_target = food_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!animating)
        {
            holopal.Nav_Agent.SetDestination(food_target.transform.position);
        }
    }

    public void OnEnter(HoloPal holopal)
    {
        animating = false;
    }

    public void OnExit(HoloPal holopal)
    {
        animating = false;
        holopal.food += 0.4f;
        holopal.Nav_Agent.isStopped = false;
        holopal.Spawner.FoodObjects.Remove(food_target);
        Object.Destroy(food_target);
        holopal.held_object = null;
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == food_target && !animating)
        {
            if (holopal.food < 0.8f)
            {
                holopal.held_object = food_target;
                holopal.Nav_Agent.isStopped = true;
                holopal.Nav_Agent.velocity = Vector3.zero;
                holopal.animator.SetTrigger("eating");
                animating = true;
            }
        }
    }
}
