using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoloStateFSM : MonoBehaviour
{
    IState current;

    // Update is called once per frame
    void Update()
    {
        current.UpdateState();
    }

    public void ChangeState(IState newState)
    {
        current.OnExit();
        current = newState;
        current.OnEnter();
    }
}

public interface IState
{
    public void UpdateState();

    public void OnEnter();

    public void OnExit();
}