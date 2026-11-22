using UnityEngine;

public partial class MainGameSceneCtrl
{
    public class MainGameInitState : StateBase<MainGameSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("MainGameInitState.OnEnter");

            owner.SetState<MainGameMainState>();
        }

        public override void OnUpdate()
        {
            Debug.Log("MainGameInitState.OnUpdate");
        }
    }
}
