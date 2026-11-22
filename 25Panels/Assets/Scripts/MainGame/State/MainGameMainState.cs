using UnityEngine;


public partial class MainGameSceneCtrl
{
    public class MainGameMainState : StateBase<MainGameSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("MainGameMainState.OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("MainGameMainState.OnUpdate");

            if (owner.IsClear)
            {
                owner.SetState<MainGameEndState>();
            }
        }
    }
}
