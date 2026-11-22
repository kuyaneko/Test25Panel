using UnityEngine;

public partial class ResultSceneCtrl
{
    public class ResultInitState : StateBase<ResultSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("ResultInitState.OnEnter");

            owner.SetState<ResultMainState>();
        }

        public override void OnUpdate()
        {
            Debug.Log("ResultInitState.OnUpdate");
        }
    }
}
