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
        if (!holopal.Nav_Agent.hasPath)
        {
            if (holopal.Hunger < 0.8f && holopal.Spawner.FoodObjects.Count > 0)
            {
                holopal.ChangeState(new EatState(holopal.Spawner.FoodObjects[0]));
                return;
            }

            if (holopal.Thirst < 0.8f && holopal.Spawner.WaterObjects.Count > 0)
            {
                holopal.ChangeState(new DrinkState(holopal.Spawner.WaterObjects[0]));
                return;
            }

            if (holopal.GameManager.Dirtiness > 80 && holopal.Spawner.CleanObjects.Count > 0)
            {
                holopal.ChangeState(new CleanState(holopal.Spawner.CleanObjects[0]));
                return;
            }

            if (timer <= 0f)
            {
                holopal.Nav_Agent.SetDestination(wander_points[Random.Range(0, wander_points.Length)]);

                timer = duration;
            }

            timer -= Time.deltaTime;
        }
    }

    public void OnEnter(HoloPal holopal)
    {
        timer = duration;
    }

    public void OnExit(HoloPal holopal)
    {

    }

    public void OnTriggerEnter(HoloPal holopal, Collider other)
    {

    }
}
