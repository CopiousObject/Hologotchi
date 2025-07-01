using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatState : IState
{
    GameObject food_target;

    public EatState(GameObject food_target)
    {
        this.food_target = food_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!food_target || holopal.hunger >= 0.8f)
        {
            holopal.ChangeState(null);
            return;
        }

        holopal.nav_agent.SetDestination(food_target.transform.position);
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {

    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == food_target)
        {
            if (holopal.hunger < 0.8f)
            {
                holopal.food_points += 10;
                holopal.spawner.FoodObjects.Remove(food_target);
                Object.Destroy(food_target);
            }
        }
    }
}
