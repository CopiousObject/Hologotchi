using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanState : IState
{
    GameObject clean_target;

    public CleanState(GameObject clean_target)
    {
        this.clean_target = clean_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!clean_target)
        {
            holopal.ChangeState(null);
            return;
        }
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
            //Play cleaning anim
            holopal.GameManager.Dirtiness = 0f;
            holopal.Spawner.CleanObjects.Remove(clean_target);
            Object.Destroy(other.gameObject);
        }
    }
}
