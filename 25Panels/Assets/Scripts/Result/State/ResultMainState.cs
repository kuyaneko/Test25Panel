using UnityEngine;


public partial class ResultSceneCtrl
{
    public class ResultMainState : StateBase<ResultSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("ResultMainState.OnEnter");

            owner.SetState<ResultEndState>();
        }

        public override void OnUpdate()
        {
            Debug.Log("ResultMainState.OnUpdate");
        }
    }
}