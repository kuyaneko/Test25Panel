using UnityEngine;

public partial class TitleSceneCtrl
{
    public class TitleInitState : StateBase<TitleSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("TitleInitState.OnEnter");

            owner.SetState<TitleMainState>();
        }

        public override void OnUpdate()
        {
            Debug.Log("TitleInitState.OnUpdate");
        }
    }
}
