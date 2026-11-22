using UnityEngine;


public partial class TitleSceneCtrl
{
    public class TitleMainState : StateBase<TitleSceneCtrl>
    {
        public override void OnEnter()
        {
            Debug.Log("TitleMainState.OnEnter");
        }

        public override void OnUpdate()
        {
            Debug.Log("TitleMainState.OnUpdate");

            if (UnityEngine.InputSystem.Mouse.current != null &&
                UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                owner.SetState<TitleEndState>();
            }
        }
    }
}
