using UnityEngine;

public class WanderState : IState
{
    private float speed;
    private float cooldown;

    private float timer;
    private Vector3 direction;

    public WanderState(float speed, float cooldown)
    {
        this.speed = speed;
        this.cooldown = cooldown;
    }

    public void UpdateState(HoloPal holopal)
    {
        if (timer <= 0f)
        {
            timer = cooldown;
            direction = Random.onUnitSphere;
        }

        holopal.transform.position += direction * (speed * Time.deltaTime);
        timer -= Mathf.Min(timer, Time.deltaTime);
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }
}
