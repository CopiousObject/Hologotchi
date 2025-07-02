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
        holopal.Nav_Agent.SetDestination(clean_target.transform.position);
        //Play cleaning anim
        OnExit(holopal);
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {
        GameObject.FindAnyObjectByType<GameManager>().Dirtiness = 0;
        holopal.ChangeState(null);
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {

    }
}
