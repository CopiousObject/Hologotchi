using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanState : IState
{
    public void UpdateState(HoloPal holopal)
    {
        //Play cleaning anim
        OnExit(holopal);
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {
        GameObject.FindAnyObjectByType<GameManager>().dirtyness = 0;
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {

    }
}
