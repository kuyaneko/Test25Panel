using UnityEngine;


public partial class MainGameSceneCtrl
{
    public class MainGameMainState : StateBase<MainGameSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("MainGameMainState.OnEnter");

            owner.SetState<MainGameEndState>();
        }

        public override void OnUpdate()
        {
            Debug.Log("MainGameMainState.OnUpdate");
        }
    }
}