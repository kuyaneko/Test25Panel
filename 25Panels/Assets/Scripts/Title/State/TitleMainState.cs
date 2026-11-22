using UnityEngine;


public partial class TitleSceneCtrl
{
    public class TitleMainState : StateBase<TitleSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("TitleMainState.OnEnter");

            owner.SetState<TitleEndState>();
        }

        public override void OnUpdate()
        {
            Debug.Log("TitleMainState.OnUpdate");
        }
    }
}