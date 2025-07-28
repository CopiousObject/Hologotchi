using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void UpdateState(HoloPal holopal);

    public void OnEnter(HoloPal holopal);

    public void OnExit(HoloPal holopal);

    public void OnTriggerEnter(HoloPal holopal, Collider other);
}

