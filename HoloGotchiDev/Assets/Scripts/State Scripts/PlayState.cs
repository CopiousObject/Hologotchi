using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : IState
{
    GameObject ball_target;

    bool has_ball;

    public PlayState(GameObject ball_target)
    {
        this.ball_target = ball_target;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (!ball_target || holopal.Playfulness >= 0.8f)
        {
            holopal.ChangeState(null);
            return;
        }

        if (!has_ball)
        {
            holopal.Nav_Agent.SetDestination(ball_target.transform.position);
        }
    }

    public void OnEnter(HoloPal holopal)
    {

    }

    public void OnExit(HoloPal holopal)
    {
        holopal.Spawner.FoodObjects.Remove(ball_target);
        Object.Destroy(ball_target);
    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {
        if (other.gameObject == ball_target)
        {
            has_ball = true;
            holopal.Nav_Agent.SetDestination(holopal.Play_position);

            ball_target.transform.SetParent(holopal.transform);
            ball_target.transform.SetLocalPositionAndRotation(new Vector3(0, 1, 1), Quaternion.identity);

            holopal.Communicator.SendData("Picked up ball");
        }
    }
}
