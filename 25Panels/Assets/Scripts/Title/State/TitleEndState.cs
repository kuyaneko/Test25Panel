using UnityEngine;

public partial class TitleSceneCtrl
{
    public class TitleEndState : StateBase<TitleSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("TitleEndState.OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("TitleEndState.OnUpdate");
        }
    }
}