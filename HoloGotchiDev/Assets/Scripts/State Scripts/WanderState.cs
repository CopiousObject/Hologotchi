using UnityEngine;

public class WanderState : IState
{
    private float duration;
    private Vector3[] wander_points;

    private float timer;

    public WanderState(float duration, Vector3[] wander_points)
    {
        this.duration = duration;
        this.wander_points = wander_points;
    }

    public void UpdateState(HoloPal holopal)
    {
        // move left-right for now
        if (!holopal.nav_agent.hasPath)
        {
            if (holopal.hunger < 0.8f && holopal.game_manager.food_objects.Count > 0)
            {
                holopal.nav_agent.SetDestination(holopal.game_manager.food_objects[0].transform.position);
                return;
            }

            if (timer <= 0f)
            {
                holopal.nav_agent.SetDestination(wander_points[Random.Range(0, wander_points.Length)]);

                timer = duration;
            }

            timer -= Time.deltaTime;
        }
    }

    public void OnEnter()
    {
        timer = duration;
    }

    public void OnExit()
    {

    }
}
