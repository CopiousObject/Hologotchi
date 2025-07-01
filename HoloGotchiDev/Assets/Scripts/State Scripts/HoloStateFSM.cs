using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HoloStateFSM : MonoBehaviour
{
    [SerializeField] public HoloPal holopal;
    // public PlayState playState = new PlayState();
    // public WanderState wanderState = new WanderState();
    // public SeekState seekState = new SeekState();
    // public DrinkState drinkState = new DrinkState();
    // public EatState eatState = new EatState();
    // public TalkState talkState = new TalkState();
    // public SickState sickState = new SickState();
    // public EvolveState evolveState = new EvolveState();
    // public LeaveState leaveState = new LeaveState();
    // public CleanState cleanState = new CleanState();
    // public ForageState forageState = new ForageState();
    // public HideSeekState hideSeekState = new HideSeekState();
    // public CreateState createState = new CreateState();
    IState current;

    // Update is called once per frame
    void Update()
    {
        current.UpdateState(holopal);
    }


}

public interface IState
{
    public void UpdateState(HoloPal holopal);

    public void OnEnter();

    public void OnExit();
}
