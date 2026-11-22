using UnityEngine;

public partial class ResultSceneCtrl
{
    public class ResultEndState : StateBase<ResultSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("ResultEndState.OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("ResultEndState.OnUpdate");
        }
    }
}