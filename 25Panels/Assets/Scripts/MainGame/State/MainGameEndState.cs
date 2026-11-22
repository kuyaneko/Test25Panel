using UnityEngine;

public partial class MainGameSceneCtrl
{
    public class MainGameEndState : StateBase<MainGameSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("MainGameEndState.OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("MainGameEndState.OnUpdate");
        }
    }
}