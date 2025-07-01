using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkState : IState
{
    GameObject water_target;

    public DrinkState(GameObject water_target)
    {
        this.water_target = water_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!water_target || holopal.thirst >= 0.8f)
        {
            holopal.ChangeState(null);
            return;
        }

        holopal.nav_agent.SetDestination(water_target.transform.position);
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {

    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == water_target)
        {
            if (holopal.thirst < 0.8f)
            {
                holopal.water_points += 10;
                holopal.spawner.WaterObjects.Remove(water_target);
                Object.Destroy(water_target);
            }
        }
    }
}
