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
        if (!water_target || holopal.water >= 0.8f)
        {
            holopal.ChangeState(null);
            return;
        }

        holopal.Nav_Agent.SetDestination(water_target.transform.position);
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
            if (holopal.water < 0.8f)
            {
                holopal.water = 1f;
                holopal.Spawner.WaterObjects.Remove(water_target);
                Object.Destroy(water_target);
            }
        }
    }
}
