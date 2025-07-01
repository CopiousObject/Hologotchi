using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanState : IState
{
    public void UpdateState(HoloPal holopal)
    {
        //Play cleaning anim
        OnExit();
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
        GameObject.FindAnyObjectByType<GameManager>().dirtyness = 0;
    }
}
